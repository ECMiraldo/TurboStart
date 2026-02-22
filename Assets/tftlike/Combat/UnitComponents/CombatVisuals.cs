using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class CombatVisuals : MonoBehaviour
{
    [SerializeField] private AttackAnimator attackAnimator;
    [SerializeField] private float maxLeanAngle = 20f;
    [SerializeField] private float offsetAmount = 0.25f;
    [SerializeField] private float offsetLerpSpeed = 12f;
    [SerializeField] private float leanLerpSpeed = 10f;
   
    private Vector3 desiredOffset = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private int moveHash = Animator.StringToHash("IsMoving");
    private int specialHash = Animator.StringToHash("Special");
    private int hurtHash = Animator.StringToHash("Hurt");
    private int diedHash = Animator.StringToHash("Died");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ResetVisuals();
    }

    public void OnAttack(UnitContext ctx, Action<UnitContext> callback)
    {
        UpdateVisuals(ctx.Grid.anchorCell, ctx.Target.Grid.anchorCell);
        if (attackAnimator != null) 
            StartCoroutine(attackAnimator.OnAttack(ctx, callback));
    }

    public void OnDamageTaken(UnitContext ctx)
    {
        if (ctx.Health.IsDead)
        {
            animator.SetTrigger(diedHash);
        }
        else
        {
            animator.SetTrigger(hurtHash);
        }

    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(
             transform.localPosition,
             desiredOffset,
             Time.deltaTime * offsetLerpSpeed
        );

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * leanLerpSpeed
        );
    }

    public void UpdateVisuals(Vector2Int self, Vector2Int target)
    {
        Vector2Int delta = target - self;
        bool horizontalCombat = Mathf.Abs(delta.x) >= Mathf.Abs(delta.y);

        // Facing
        if (horizontalCombat && delta.x != 0)
        {
            spriteRenderer.flipX = delta.x < 0;
        }

        // Vertical lean
        float vertical = Mathf.Clamp(delta.y, -1f, 1f);
        float direction = spriteRenderer.flipX ? 1f : -1f;
        float lean = vertical * maxLeanAngle * -direction;
        targetRotation = Quaternion.Euler(0, 0, lean);

        // Intra-cell offset
        desiredOffset = Vector3.zero;

        Vector2 dir = (target - self);

        if (dir.sqrMagnitude > 0.0001f)
        {
            dir.Normalize();
            desiredOffset = new Vector3(dir.x, dir.y, 0f) * offsetAmount;
        }
    
    }
    public void ResetVisuals()
    {
         transform.localRotation = Quaternion.Lerp(
             transform.localRotation,
             Quaternion.identity,
             Time.deltaTime * leanLerpSpeed
        );

        transform.localPosition = Vector3.Lerp(
             transform.localPosition,
             Vector3.zero,
             Time.deltaTime * offsetLerpSpeed
        );
    }
}
