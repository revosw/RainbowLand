using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// ColorFilter is a script that should be attached to a Global Volume game object
/// </summary>
public class ColorFilter : MonoBehaviour
{
    public enum Filter { Gray, OnlyRed, RedGreen, AllColors}
    [SerializeField] AnimationCurve Gray;
    [SerializeField] AnimationCurve OnlyRed;
    [SerializeField] AnimationCurve RedGreen;
    [SerializeField] AnimationCurve AllColors;
    private Bloom bloom = null;
    private ColorCurves colorCurves = null;
    private TextureCurve activeCurve;
    private Keyframe redOffset;
    private Keyframe greenOffset;
    private Keyframe blueOffset;

    // Start is called before the first frame update
    void Start()
    {
        Volume volume = GetComponent<Volume>();

        volume.sharedProfile.TryGet(out bloom);
        volume.sharedProfile.TryGet(out colorCurves);

        SetFilter(Filter.Gray);
    }

    public void SetFilter(Filter filter)
    {
        activeCurve = filter switch
        {
            Filter.Gray => new TextureCurve(Gray, 0f, true, new Vector2(0f, 1f)),
            Filter.OnlyRed => new TextureCurve(OnlyRed, 0f, true, new Vector2(0f, 1f)),
            Filter.RedGreen => new TextureCurve(RedGreen, 0f, true, new Vector2(0f, 1f)),
            Filter.AllColors => new TextureCurve(AllColors, 0f, true, new Vector2(0f, 1f)),
        };

        redOffset = activeCurve[0];
        greenOffset = activeCurve[1];
        blueOffset = activeCurve[2];

        colorCurves.hueVsSat.Override(activeCurve);
    }

    // Update is called once per frame
    void Update()
    {
        bloom.intensity.value = 5f + Mathf.Sin(Time.time * 0.6f) * 3;
        Keyframe red = new Keyframe(0, redOffset.value + Mathf.Sin(Time.time * 1.2f) * 0.4f);
        Keyframe green = new Keyframe(0.5f, greenOffset.value + Mathf.Sin(Time.time * 0.5f) * 0.5f);
        Keyframe blue = new Keyframe(1f, blueOffset.value + Mathf.Sin((Time.time + 0.8f) * 1.2f));
        activeCurve.MoveKey(0, red);
        activeCurve.MoveKey(1, green);
        activeCurve.MoveKey(2, blue);
    }
}