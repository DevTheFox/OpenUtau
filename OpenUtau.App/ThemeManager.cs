﻿using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;

namespace OpenUtau.App {
    class ThemeManager {
        static public string[] noteStrings = new String[12] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        static readonly int[] blackKeys = { 1, 3, 6, 8, 10 };

        // Window UI
        public static SolidColorBrush UIBackgroundBrushNormal = new SolidColorBrush();
        public static SolidColorBrush UIBackgroundBrushActive = new SolidColorBrush();
        public static SolidColorBrush UINeutralBrushNormal = new SolidColorBrush();
        public static SolidColorBrush UINeutralBrushActive = new SolidColorBrush();
        public static SolidColorBrush ForegroundBrushNormal = new SolidColorBrush();
        public static SolidColorBrush ForegroundBrushActive = new SolidColorBrush();

        // Midi editor background
        public static LinearGradientBrush WhiteKeyBrushNormal = new LinearGradientBrush() { };
        public static SolidColorBrush WhiteKeyNameBrushNormal = new SolidColorBrush();
        public static LinearGradientBrush BlackKeyBrushNormal = new LinearGradientBrush() { };
        public static SolidColorBrush BlackKeyNameBrushNormal = new SolidColorBrush();
        public static LinearGradientBrush CenterKeyBrushNormal = new LinearGradientBrush() { };
        public static SolidColorBrush CenterKeyNameBrushNormal = new SolidColorBrush();

        public static SolidColorBrush TrackBackgroundBrush = new SolidColorBrush();
        public static SolidColorBrush TrackBackgroundBrushAlt = new SolidColorBrush();

        public static SolidColorBrush TickLineBrushLight = new SolidColorBrush();
        public static SolidColorBrush TickLineBrushDark = new SolidColorBrush();
        public static SolidColorBrush BarNumberBrush = new SolidColorBrush();

        // Midi editor markers
        public static SolidColorBrush PlayPosMarkerHighlightBrush = new SolidColorBrush();

        // Midi notes
        public static SolidColorBrush NoteFillSelectedBrush = new SolidColorBrush();
        public static SolidColorBrush NoteFillSelectedErrorBrushes = new SolidColorBrush();
        public static SolidColorBrush NoteStrokeSelectedBrush = new SolidColorBrush();
        public static SolidColorBrush NoteStrokeErrorBrush = new SolidColorBrush();

        public static List<SolidColorBrush> NoteFillBrushes = new List<SolidColorBrush>();
        public static List<SolidColorBrush> NoteStrokeBrushes = new List<SolidColorBrush>();
        public static List<SolidColorBrush> NoteFillErrorBrushes = new List<SolidColorBrush>();

        public static SolidColorBrush PitchBrush = new SolidColorBrush();
        public static SolidColorBrush VibratoBrush = new SolidColorBrush();

        public static bool LoadTheme() {

            const int NumberOfChannel = 1;

            WhiteKeyBrushNormal.GradientStops.Add(new GradientStop(GetColor("WhiteKeyColorNormalLeft"), 0));
            WhiteKeyBrushNormal.GradientStops.Add(new GradientStop(GetColor("WhiteKeyColorNormalRight"), 1));
            WhiteKeyNameBrushNormal.Color = GetColor("WhiteKeyNameColorNormal");

            BlackKeyBrushNormal.GradientStops.Add(new GradientStop(GetColor("BlackKeyColorNormalLeft"), 0));
            BlackKeyBrushNormal.GradientStops.Add(new GradientStop(GetColor("BlackKeyColorNormalRight"), 1));
            BlackKeyNameBrushNormal.Color = GetColor("BlackKeyNameColorNormal");

            CenterKeyBrushNormal.GradientStops.Add(new GradientStop(GetColor("CenterKeyColorNormalLeft"), 0));
            CenterKeyBrushNormal.GradientStops.Add(new GradientStop(GetColor("CenterKeyColorNormalLeft"), 1));
            CenterKeyNameBrushNormal.Color = GetColor("CenterKeyNameColorNormal");

            UIBackgroundBrushNormal.Color = GetColor("UIBackgroundColorNormal");
            UIBackgroundBrushActive.Color = GetColor("UIBackgroundColorActive");
            UINeutralBrushNormal.Color = GetColor("UINeutralColorNormal");
            UINeutralBrushActive.Color = GetColor("UINeutralColorActive");
            ForegroundBrushNormal.Color = GetColor("ForegroundColorNormal");
            ForegroundBrushActive.Color = GetColor("ForegroundColorActive");

            TrackBackgroundBrush.Color = GetColor("TrackBackgroundColor");
            TrackBackgroundBrushAlt.Color = GetColor("TrackBackgroundColorAlt");

            TickLineBrushLight.Color = GetColor("TickLineColorLight");
            TickLineBrushDark.Color = GetColor("TickLineColorDark");
            BarNumberBrush.Color = GetColor("BarNumberColor");

            PlayPosMarkerHighlightBrush.Color = GetColor("PlayPosMarkerHighlightColor");

            // Midi notes
            NoteFillSelectedBrush.Color = GetColor("NoteFillSelectedColorB");
            NoteFillSelectedErrorBrushes.Color = GetColorVariationAlpha(NoteFillSelectedBrush.Color, 127);

            NoteStrokeSelectedBrush.Color = GetColor("NoteStrokeSelectedColor");
            NoteStrokeErrorBrush.Color = GetColor("NoteStrokeErrorColor");

            for (int i = 0; i < NumberOfChannel; i++) {
                NoteFillBrushes.Add(new SolidColorBrush());
                NoteStrokeBrushes.Add(new SolidColorBrush());
                NoteFillErrorBrushes.Add(new SolidColorBrush());

                NoteFillBrushes[i].Color = GetColor("NoteFillColorBCh" + i);
                NoteFillErrorBrushes[i].Color = GetColorVariationAlpha(NoteFillBrushes[i].Color, 127);
                NoteStrokeBrushes[i].Color = GetColor("NoteStrokeColorCh" + i);
            }

            PitchBrush.Color = GetColor("PitchBrushColor");
            VibratoBrush.Color = GetColor("VibratoBrushColor");

            return true;
        }

        public static Color GetColor(string name) {
            //return (Color)Application.Current.FindResource(name);
            return Colors.Wheat;
        }

        public static Color GetColorVariationAlpha(Color color, byte alpha) {
            return new Color(color.R, color.G, color.B, alpha);
        }
    }
}
