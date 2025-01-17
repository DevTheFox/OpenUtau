﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using DynamicData.Binding;
using OpenUtau.Core;
using OpenUtau.Core.Ustx;
using ReactiveUI;

namespace OpenUtau.App.ViewModels {
    public class ExpressionBuilder {
        private static readonly string[] required = { "vel", "vol", "acc", "dec" };

        public string Name { get; set; } = string.Empty;
        public string Abbr { get; set; } = string.Empty;
        public float Min { get; set; }
        public float Max { get; set; } = 100;
        public float DefaultValue { get; set; }
        public string Flag { get; set; } = string.Empty;
        public bool IsCustom => !required.Contains(Abbr);

        public ExpressionBuilder(UExpressionDescriptor descriptor) {
            Name = descriptor.name;
            Abbr = descriptor.abbr;
            Min = descriptor.min;
            Max = descriptor.max;
            DefaultValue = descriptor.defaultValue;
            Flag = descriptor.flag;
        }

        public ExpressionBuilder() {
            Name = "new expression";
        }

        public bool IsValid() {
            return !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Abbr)
                && Abbr.Trim().Length == 3
                && Min < Max
                && Min <= DefaultValue
                && DefaultValue <= Max;
        }

        public UExpressionDescriptor Build() {
            return new UExpressionDescriptor(Name.Trim(), Abbr.Trim().ToLower(), Min, Max, DefaultValue, Flag);
        }

        public override string ToString() => Name;
    }

    public class ExpressionsViewModel : ViewModelBase {

        public ReadOnlyObservableCollection<ExpressionBuilder> Expressions => expressions;

        public ExpressionBuilder? Expression {
            get => expression;
            set => this.RaiseAndSetIfChanged(ref expression, value);
        }

        private ReadOnlyObservableCollection<ExpressionBuilder> expressions;
        private ExpressionBuilder? expression;

        private ObservableCollectionExtended<ExpressionBuilder> expressionsSource;

        public ExpressionsViewModel() {
            expressionsSource = new ObservableCollectionExtended<ExpressionBuilder>();
            expressionsSource.ToObservableChangeSet()
                .Bind(out expressions)
                .Subscribe();
            expressionsSource.AddRange(DocManager.Inst.Project.expressions.Select(pair => new ExpressionBuilder(pair.Value)));
            if (expressionsSource.Count > 0) {
                expression = expressionsSource[0];
            }
        }

        public void Apply() {
            if (!Expressions.All(builder => builder.IsValid())) {
                var invalid = Expressions.First(builder => !builder.IsValid());
                Expression = invalid;
                if (string.IsNullOrWhiteSpace(invalid.Name)) {
                    throw new ArgumentException("Name must be set.");
                } else if (string.IsNullOrWhiteSpace(invalid.Abbr)) {
                    throw new ArgumentException("Abbreviation must be set.");
                } else if (invalid.Abbr.Trim().Length != 3) {
                    throw new ArgumentException("Abbreviation must be 3 characters long.");
                } else {
                    throw new ArgumentException("Invalid min, max or default Value.");
                }
            }
            var abbrs = Expressions.Select(builder => builder.Abbr);
            if (abbrs.Count() != abbrs.Distinct().Count()) {
                throw new ArgumentException("Abbreviations must be unique.");
            }
            var flags = Expressions.Where(builder => !string.IsNullOrEmpty(builder.Flag)).Select(builder => builder.Flag);
            if (flags.Count() != flags.Distinct().Count()) {
                throw new ArgumentException("Flags must be unique.");
            }
            DocManager.Inst.StartUndoGroup();
            DocManager.Inst.ExecuteCmd(new ConfigureExpressionsCommand(DocManager.Inst.Project, Expressions.Select(builder => builder.Build()).ToArray()));
            DocManager.Inst.EndUndoGroup();
        }

        public void Add() {
            var newExpression = new ExpressionBuilder();
            expressionsSource.Add(newExpression);
            Expression = newExpression;
        }

        public void Remove() {
            if (Expression != null) {
                expressionsSource.Remove(Expression);
            }
        }
    }
}
