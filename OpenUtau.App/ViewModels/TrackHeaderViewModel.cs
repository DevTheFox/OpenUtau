﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using OpenUtau.Api;
using OpenUtau.Core;
using OpenUtau.Core.Ustx;
using ReactiveUI;

namespace OpenUtau.App.ViewModels {
    public class TrackHeaderViewModel : ViewModelBase {
        public int TrackNo => track.TrackNo;
        public USinger Singer => track.Singer;
        public Phonemizer Phonemizer => track.Phonemizer;
        public string PhonemizerTag => track.Phonemizer.Tag;
        public IReadOnlyList<MenuItemViewModel> SingerMenuItems { get; set; }
        public ReactiveCommand<USinger, Unit> SelectSingerCommand { get; }
        public IReadOnlyList<MenuItemViewModel> PhonemizerMenuItems { get; set; }
        public ReactiveCommand<PhonemizerFactory, Unit> SelectPhonemizerCommand { get; }

        private readonly UTrack track;

        public TrackHeaderViewModel() {
        }

        public TrackHeaderViewModel(UTrack track) {
            this.track = track;
            SelectSingerCommand = ReactiveCommand.Create<USinger>(singer => {
                if (track.Singer != singer) {
                    DocManager.Inst.StartUndoGroup();
                    DocManager.Inst.ExecuteCmd(new TrackChangeSingerCommand(DocManager.Inst.Project, track, singer));
                    DocManager.Inst.EndUndoGroup();
                }
                this.RaisePropertyChanged(nameof(Singer));
            });
            SelectPhonemizerCommand = ReactiveCommand.Create<PhonemizerFactory>(factory => {
                if (track.Phonemizer.GetType() != factory.type) {
                    DocManager.Inst.StartUndoGroup();
                    DocManager.Inst.ExecuteCmd(new TrackChangePhonemizerCommand(DocManager.Inst.Project, track, factory.Create()));
                    DocManager.Inst.EndUndoGroup();
                }
                this.RaisePropertyChanged(nameof(Phonemizer));
                this.RaisePropertyChanged(nameof(PhonemizerTag));
            });
        }

        public void RefreshSingers() {
            SingerMenuItems = DocManager.Inst.Singers.Values.Select(singer => new MenuItemViewModel() {
                Header = singer.Name,
                Command = SelectSingerCommand,
                CommandParameter = singer,
            }).ToArray();
            this.RaisePropertyChanged(nameof(SingerMenuItems));
        }

        public void RefreshPhonemizers() {
            PhonemizerMenuItems = DocManager.Inst.PhonemizerFactories.Select(factory => new MenuItemViewModel() {
                Header = factory.ToString(),
                Command = SelectPhonemizerCommand,
                CommandParameter = factory,
            }).ToArray();
            this.RaisePropertyChanged(nameof(PhonemizerMenuItems));
        }

        public void ManuallyRaise() {
            this.RaisePropertyChanged(nameof(Singer));
            this.RaisePropertyChanged(nameof(Phonemizer));
            this.RaisePropertyChanged(nameof(PhonemizerTag));
        }

        public void Remove() {
            DocManager.Inst.StartUndoGroup();
            DocManager.Inst.ExecuteCmd(new RemoveTrackCommand(DocManager.Inst.Project, track));
            DocManager.Inst.EndUndoGroup();
        }

        public void MoveUp() {
            if (track == DocManager.Inst.Project.tracks.First()) {
                return;
            }
            DocManager.Inst.StartUndoGroup();
            DocManager.Inst.ExecuteCmd(new MoveTrackCommand(DocManager.Inst.Project, track, true));
            DocManager.Inst.EndUndoGroup();
        }

        public void MoveDown() {
            if (track == DocManager.Inst.Project.tracks.Last()) {
                return;
            }
            DocManager.Inst.StartUndoGroup();
            DocManager.Inst.ExecuteCmd(new MoveTrackCommand(DocManager.Inst.Project, track, false));
            DocManager.Inst.EndUndoGroup();
        }
    }
}
