using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    void OnEnable() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    void OnDisable() {
        UpdateOutline(false);
    }

    void UpdateOutline(bool outline) {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Enable", outline ? 1f : 0);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}
