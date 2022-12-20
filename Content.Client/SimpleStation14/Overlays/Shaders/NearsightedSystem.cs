using Content.Shared.Abilities;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Robust.Shared.Network;
using Content.Shared.Inventory.Events;
using Content.Shared.Tag;
using Content.Shared.Inventory;
using Content.Client.Inventory;

namespace Content.Client.SimpleStation14.Overlays;
public sealed class NearsightedSystem : EntitySystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IOverlayManager _overlayMan = default!;
    [Dependency] private readonly INetManager _net = default!;

    private NearsightedOverlay _overlay = default!;
    private NearsightedComponent nearsight = new();

    public override void Initialize()
    {
        base.Initialize();

        _overlay = new Overlays.NearsightedOverlay();

        SubscribeLocalEvent<NearsightedComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(EntityUid uid, NearsightedComponent component, ExaminedEvent args)
    {
        if (args.IsInDetailsRange)
        {
            args.PushMarkup(Loc.GetString("permanent-nearsighted-trait-examined", ("target", Identity.Entity(uid, EntityManager))));
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        foreach (var nearsight in EntityQuery<NearsightedComponent>())
        {
            var sighted = nearsight.Owner;

            var cinv = EnsureComp<ClientInventoryComponent>(sighted);
            cinv.SlotData.TryGetValue("eyes", out var eyes);
            var eyeslot = eyes?.Container?.ContainedEntity;

            if (eyeslot == null) UpdateShader(nearsight);
            else
            {
                EntityUid eyeslo = new();
                eyeslo = (EntityUid) eyeslot;

                var comp = EnsureComp<TagComponent>(eyeslo);
                if (comp.Tags.Contains("GlassesNearsight")) UpdateShaderGlasses(nearsight);
            }
        }
    }


    private void UpdateShader(NearsightedComponent component)
    {
        while (_overlayMan.HasOverlay<NearsightedOverlay>()) _overlayMan.RemoveOverlay(_overlay);
        component.Glasses = false;
        _overlayMan.AddOverlay(_overlay);
    }

    private void UpdateShaderGlasses(NearsightedComponent component)
    {
        while (_overlayMan.HasOverlay<NearsightedOverlay>()) _overlayMan.RemoveOverlay(_overlay);
        component.Glasses = true;
        _overlayMan.AddOverlay(_overlay);
    }
}