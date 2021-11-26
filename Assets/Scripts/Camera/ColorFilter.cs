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

    // Start is called before the first frame update
    void Start()
    {
        Volume volume = GetComponent<Volume>();

        volume.sharedProfile.TryGet(out bloom);
        volume.sharedProfile.TryGet(out colorCurves);
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
        
        colorCurves.hueVsSat.Override(activeCurve);
    }

    // Update is called once per frame
    void Update()
    {
        Keyframe red = new Keyframe(0, Mathf.Sin(Time.time));
        Keyframe green = new Keyframe(0.5f, Mathf.Sin(Time.time + 0.5f));
        Keyframe blue = new Keyframe(1f, Mathf.Sin(Time.time + 0.8f * 1.2f));
        activeCurve.MoveKey(0, red);
        activeCurve.MoveKey(1, green);
        activeCurve.MoveKey(2, blue);
    }
}