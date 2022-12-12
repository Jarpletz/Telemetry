#ifndef GRADIENTBYPASS_INCLUDED
#define GRADIENTBYPASS_INCLUDED

void GradientBypass_float(float inputValue,
    float4 color1,
    float4 color2,
    float4 color3,
    float4 color4,
    float4 color5,
    float location1,
    float location2,
    float location3,
    float location4,
    float location5,
    out float4 outFloat)
{
    if (inputValue < location1||location2==0)
    {
        outFloat = color1;
    }
    if (inputValue < location2 && inputValue >= location1 && location2 > location1)
    {
        float pos = (inputValue - location1) / (location2 - location1);
        outFloat = lerp(color1, color2, pos);
    }
    if (inputValue < location3 && inputValue >= location2 && location3 > location2)
    {
        float pos = (inputValue - location2) / (location3 - location2);
        outFloat = lerp(color2, color3, pos);
    }
    if (inputValue < location4 && inputValue >= location3 && location4 > location3)
    {
        float pos = (inputValue - location3) / (location4 - location3);
        outFloat = lerp(color3, color4, pos);
    }
    if (inputValue < location5 && inputValue >= location4  && location5 > location4)
    {
        float pos = (inputValue - location4) / (location5 - location4);
        outFloat = lerp(color4, color5, pos);
    }
    if(inputValue>=location5)
    {
        outFloat = color5;
    }
}


#endif // GRADIENTBYPASS_INCLUDED