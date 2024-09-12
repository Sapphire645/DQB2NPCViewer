using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
public class OverlayShaderEffect : ShaderEffect
{
    private static readonly PixelShader _pixelShader = new PixelShader
    {
        UriSource = new Uri("pack://application:,,,/Overlay.ps")
    };

    public static readonly DependencyProperty Input1Property =
        ShaderEffect.RegisterPixelShaderSamplerProperty("Input1", typeof(OverlayShaderEffect), 0);

    public static readonly DependencyProperty Input2Property =
        ShaderEffect.RegisterPixelShaderSamplerProperty("Input2", typeof(OverlayShaderEffect), 1);

    public Brush Input1
    {
        get { return (Brush)GetValue(Input1Property); }
        set { SetValue(Input1Property, value); }
    }

    public Brush Input2
    {
        get { return (Brush)GetValue(Input2Property); }
        set { SetValue(Input2Property, value); }
    }

    public OverlayShaderEffect()
    {
        PixelShader = _pixelShader;
        UpdateShaderValue(Input1Property);
        UpdateShaderValue(Input2Property);
    }
}