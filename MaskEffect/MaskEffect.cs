using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using Tizen.NUI;
using Tizen.NUI.Components;
using Tizen.NUI.BaseComponents;

namespace MaskEffect
{
    class Program : NUIApplication
    {
        const int WINDOW_WIDTH = 1920;
        const int WINDOW_HEIGHT = 1080;
        const int IMAGE_WIDTH = 1920;
        const int IMAGE_HEIGHT = 900;
        const int LABEL_POINT_SIZE = 200;
        const int BUTTON_WIDTH = 300;
        const int BUTTON_HEIGHT = 100;
        const float positionX = 0.0f;
        const float positionY = 0.0f;
        const float scaleX = 1.0f;
        const float scaleY = 1.0f;
        Window window;
        View MainView;
        View ButtonView;
        View ImageView;
        int TargetImageMode = 1;
        int SourceImageMode = 1;
        MaskEffectMode maskMode = MaskEffectMode.Alpha;
        View TargetImage1;
        ImageView TargetImage2;
        TextLabel SourceImage1;
        ImageView SourceImage2;
        RenderEffect renderEffect;
        ColorVisual backgroundVisual1;
        ImageVisual backgroundVisual2;

        private void EnableRenderEffect()
        {
            TargetImage1.Unparent();
            TargetImage2.Unparent();

            TargetImage1.ClearRenderEffect();
            TargetImage2.ClearRenderEffect();

            if(TargetImageMode == 1)
            {
                ImageView.Add(TargetImage1);

                if(SourceImageMode == 1)
                {
                    renderEffect = RenderEffect.CreateMaskEffect(SourceImage1, maskMode, positionX, positionY, scaleX, scaleY);
                    TargetImage1.SetRenderEffect(renderEffect);
                }
                else
                {
                    renderEffect = RenderEffect.CreateMaskEffect(SourceImage2, maskMode, positionX, positionY, scaleX, scaleY);
                    TargetImage1.SetRenderEffect(renderEffect);
                }
            }
            else
            {
                ImageView.Add(TargetImage2);
                TargetImage2.Unparent();
                ImageView.Add(TargetImage2);

                if(SourceImageMode == 1)
                {
                    renderEffect = RenderEffect.CreateMaskEffect(SourceImage1, maskMode, positionX, positionY, scaleX, scaleY);
                    TargetImage2.SetRenderEffect(renderEffect);
                }
                else
                {
                    renderEffect = RenderEffect.CreateMaskEffect(SourceImage2, maskMode, positionX, positionY, scaleX, scaleY);
                    TargetImage2.SetRenderEffect(renderEffect);
                }
            }
        }
        private void MaskTarget1ButtonClicked(object sender, ClickedEventArgs args)
        {
            TargetImageMode = 1;

            EnableRenderEffect();
        }

        private void MaskTarget2ButtonClicked(object sender, ClickedEventArgs args)
        {
            TargetImageMode = 2;

            EnableRenderEffect();
        }

        private void AlphaMaskModeButtonClicked(object sender, ClickedEventArgs args)
        {
            SourceImage2.Unparent();
            ImageView.Add(SourceImage1);
            SourceImageMode = 1;

            maskMode = MaskEffectMode.Alpha;
            EnableRenderEffect();
        }

        private void LuminanceMaskModeButtonClicked(object sender, ClickedEventArgs args)
        {
            SourceImage1.Unparent();
            ImageView.Add(SourceImage2);
            SourceImageMode = 2;

            maskMode = MaskEffectMode.Luminance;
            EnableRenderEffect();
        }

        private void BlackBackgroundButtonClicked(object sender, ClickedEventArgs args)
        {
            ImageView.Background = backgroundVisual1.OutputVisualMap;
        }

        private void GradientBackgroundButtonClicked(object sender, ClickedEventArgs args)
        {
            ImageView.Background = backgroundVisual2.OutputVisualMap;
        }

        ImageView makeImageView(string url)
        {
            var image = new ImageView
            {
                ResourceUrl = url,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent,
            };

            return image;
        }

        protected override void OnCreate()
        {
            string resourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource;
            FontClient.Instance.AddCustomFontDirectory(resourcePath + "/fonts");

            window = NUIApplication.GetDefaultWindow();
            window.WindowSize = new Size(WINDOW_WIDTH, WINDOW_HEIGHT);
            window.BackgroundColor = Color.White;

            MainView = new View()
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                },
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                BackgroundColor = Color.Transparent,
                Margin = new Extents(0, 0, 6, 6),
            };

            window.Add(MainView);

            ButtonView = new View
            {
                Size2D = new Size2D(WINDOW_WIDTH, WINDOW_HEIGHT - IMAGE_HEIGHT),
                PositionUsesPivotPoint = true,
                PivotPoint = PivotPoint.TopLeft,
                ParentOrigin = ParentOrigin.TopLeft,
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                },
            };

            Button MaskTarget1Button = new Button()
            {
                Text = "MaskTarget1",
                BackgroundColor = Color.Red,
                Size2D = new Size2D(BUTTON_WIDTH, BUTTON_HEIGHT),
            };
            ButtonView.Add(MaskTarget1Button);
            MaskTarget1Button.Clicked += MaskTarget1ButtonClicked;

            Button MaskTarget2Button = new Button()
            {
                Text = "MaskTarget2",
                BackgroundColor = Color.Red,
                Size2D = new Size2D(BUTTON_WIDTH, BUTTON_HEIGHT),
            };
            ButtonView.Add(MaskTarget2Button);
            MaskTarget2Button.Clicked += MaskTarget2ButtonClicked;

            Button AlphaMaskModeButton = new Button()
            {
                Text = "AlphaMaskMode",
                BackgroundColor = Color.Blue,
                Size2D = new Size2D(BUTTON_WIDTH, BUTTON_HEIGHT),
            };
            ButtonView.Add(AlphaMaskModeButton);
            AlphaMaskModeButton.Clicked += AlphaMaskModeButtonClicked;

            Button LuminanceMaskModeButton = new Button()
            {
                Text = "LuminanceMaskMode",
                BackgroundColor = Color.Blue,
                Size2D = new Size2D(BUTTON_WIDTH, BUTTON_HEIGHT),
            };
            ButtonView.Add(LuminanceMaskModeButton);
            LuminanceMaskModeButton.Clicked += LuminanceMaskModeButtonClicked;

            Button BlackBackgroundButton = new Button()
            {
                Text = "BlackBackground",
                BackgroundColor = Color.Green,
                Size2D = new Size2D(BUTTON_WIDTH, BUTTON_HEIGHT),
            };
            ButtonView.Add(BlackBackgroundButton);
            BlackBackgroundButton.Clicked += BlackBackgroundButtonClicked;

            Button GradientBackgroundButton = new Button()
            {
                Text = "GradientBackground",
                BackgroundColor = Color.Green,
                Size2D = new Size2D(BUTTON_WIDTH, BUTTON_HEIGHT),
            };
            ButtonView.Add(GradientBackgroundButton);
            GradientBackgroundButton.Clicked += GradientBackgroundButtonClicked;

            backgroundVisual1 = new ColorVisual()
            {
                Color = Color.Black,
            };
            backgroundVisual2 = new ImageVisual()
            {
                URL = resourcePath + "/images/rainbow.jpg",
            };
            ImageView = new View
            {
                Size2D = new Size2D(IMAGE_WIDTH, IMAGE_HEIGHT),
                PositionUsesPivotPoint = true,
                PivotPoint = PivotPoint.TopLeft,
                ParentOrigin = ParentOrigin.TopLeft,

                Background = backgroundVisual1.OutputVisualMap,
            };

            //TargetImage1 = makeImageView(resourcePath + "/images/colorful.jpg");
            TargetImage1 = new View()
            {
                Size2D = new Size2D(WINDOW_WIDTH, WINDOW_HEIGHT),
                ParentOrigin = Position.ParentOriginTopLeft,
                PivotPoint = Position.PivotPointTopLeft,
                PositionUsesPivotPoint = true,
            };

            using (PropertyMap map = new PropertyMap())
            {
                map.Insert((int)Visual.Property.Type, new PropertyValue((int)Visual.Type.Gradient));
                map.Insert((int)GradientVisualProperty.StartPosition, new PropertyValue(new Vector2(0, 0)));
                map.Insert((int)GradientVisualProperty.EndPosition, new PropertyValue(new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT)));

                var stopOffsetArray = new PropertyArray();
                stopOffsetArray.Add(new PropertyValue(0.0f));
                stopOffsetArray.Add(new PropertyValue(0.5f));
                stopOffsetArray.Add(new PropertyValue(1.0f));
                map.Insert((int)GradientVisualProperty.StopOffset, new PropertyValue(stopOffsetArray));

                var stopColorArray = new PropertyArray();
                stopColorArray.Add(new PropertyValue(Color.Red));
                stopColorArray.Add(new PropertyValue(Color.Green));
                stopColorArray.Add(new PropertyValue(Color.Blue));
                map.Insert((int)GradientVisualProperty.StopColor, new PropertyValue(stopColorArray));
                map.Insert((int)GradientVisualProperty.SpreadMethod, new PropertyValue((int)GradientVisualSpreadMethodType.Pad));
                map.Insert((int)GradientVisualProperty.Units, new PropertyValue((int)GradientVisualUnitsType.UserSpace));

                map.Insert((int)GradientVisualProperty.StartOffset, new PropertyValue(0.0f));

                TargetImage1.Background = map;
            }

            TargetImage2 = makeImageView(resourcePath + "/images/ocean.gif");
            SourceImage1 = new TextLabel()
            {
                Size2D = new Size2D(IMAGE_WIDTH, IMAGE_HEIGHT),
                Text = "Hello, World!",
                PointSize = LABEL_POINT_SIZE,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            SourceImage2 = makeImageView(resourcePath + "/images/chess.jpg");

            ImageView.Add(TargetImage1);
            ImageView.Add(SourceImage1);

            renderEffect = RenderEffect.CreateMaskEffect(SourceImage1);
            TargetImage1.SetRenderEffect(renderEffect);

            MainView.Add(ButtonView);
            MainView.Add(ImageView);
            window.Add(MainView);
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
