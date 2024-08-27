using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;

namespace SnivysServerEvents.Configs;

public class ChaoticConfig
{
    public string StartEventCassieMessage { get; set; } = "Facility is unstable";
    public string StartEventCassieText { get; set; } = "Facility is unstable (Chaos Event)";
    
    [Description("Determines how long before the next chaos event (in seconds)")]
    public float TimeForChaosEvent { get; set; } = 60;
    
    [Description("The time for Broadcasts to be shown on the screen (this applies to all broadcasts that the Chaotic Event uses)")] 
    public float BroadcastDisplayTime { get; set; } = 10;

    [Description("Should the item steal chaos event run?")]
    public bool ItemStealEvent { get; set; } = true;
    [Description("What should the broadcast say when players items has been stolen")]
    public string ItemStolenText { get; set; } = "It has appears that your items has been stolen";

    [Description("Should give random items chaos event run?")]
    public bool GiveRandomItemEvent { get; set; } = true;

    [Description("What should the broadcast say when an item is given")]
    public string GiveRandomItemText { get; set; } = "It has appears that you have obtained an item";

    [Description("Should random items only be regular items or use custom items as well?")]
    public bool GiveRandomItemCustomitems { get; set; } = false;

    [Description("Should random teleport chaos event run?")]
    public bool RandomTeleportEvent { get; set; } = true;

    [Description(
        "If the Random Teleport Event is active, should players be teleported to Light after decomtamination?")]
    public bool TeleportToLightAfterDecom { get; set; } = false;

    [Description(
        "If the Random Teleport Event is active, should players be teleported into the facility after the warhead detonated?")]
    public bool TeleportToFacilityAfterNuke { get; set; } = false;

    [Description("What should the broadcast say when a player gets teleported?")]
    public string RandomTeleportText { get; set; } = "Whoops, my bad";

    [Description("Should the fake autonuke chaos event run?")]
    public bool FakeAutoNuke { get; set; } = true;

    [Description(
        "What should the broadcast say for when the fake auto nuke starts (this should match your real autonuke text to make it more conviencing")]
    public string FakeAutoNukeStartText { get; set; } = "Autonuke has started";

    [Description("What should the broadcast say for when it is reviealed that the auto nuke was fake")]
    public string FakeAutoNukeFakeoutText { get; set; } = "Get pranked";

    [Description(
        "What should be the time before the nuke goes off before its shown to be a fake, note that anything below 10 seconds wont work")]
    public float FakeAutoNukeTimeOut { get; set; } = 15f;

    [Description("Should the remove weapons chaos event run?")]
    public bool RemoveWeaponsEvent { get; set; } = true;

    [Description("What should the broadcast text say when a weapon is taken away?")]
    public string RemoveWeaponsText { get; set; } = "I hope you didnt plan on entering a fight soon";

    [Description("Should the give random weapon event be active?")]
    public bool GiveRandomWeaponsEvent { get; set; } = true;

    [Description("Should the random weapon giving only apply to people who dont have weapons?")]
    public bool GiveRandomWeaponsToUnarmedPlayers { get; set; } = true;

    [Description("What should the broadcast text say when a weapon is given?")]
    public string GiveRandomWeaponsText { get; set; } = "Here have a weapon";

    [Description("What should the broadcast say if the player's inventory is full and cannot recieve a weapon")]
    public string GiveRandomWeaponsTextFail { get; set; } = "No free weaponry for you, bawomp";

    [Description("Should all weapons be up for grabs?")]
    public bool GiveAllRandomWeapons { get; set; } = true;

    [Description("If giving all weapons is false, what weapons can be given?")]
    [CanBeNull]
    public List<ItemType> GiveRandomWeaponsDefined { get; set; } = new List<ItemType>()
    {
        ItemType.GunCOM15,
        ItemType.GunCOM18,
        ItemType.GunCrossvec
    };

    [Description("Should the death match event be active?")]
    public bool DeathMatchEvent { get; set; } = true;

    [Description("What should the broadcast say when players health is reduced?")]
    public string DeathMatchText { get; set; } = "*Sneezes* Sorry I hope I didnt mess anything up";

    [Description("Should SCPs be affected by this as well?")]
    public bool DeathMatchEventAffectsSCPs { get; set; } = false;

    [Description("What health value should players health be set at?")]
    public float DeathMatchHealth { get; set; } = 1f;

    [Description("Should the Chaos Event be able to enable other events?")]
    public bool ChaosEventEnablesOtherEvents { get; set; } = true;

    [Description("Should the other events that get activated end when the next cycle comes along?")]
    public bool ChaosEventEndsOtherEvents { get; set; } = false;

    [Description("Should the FBI Open Up Event be active?")]
    public bool FBIOpenUpEvent { get; set; } = true;

    [Description("What should the broadcast say to the target that's going to be teleported too")]
    public string FBIOpenUpTargetText { get; set; } = "The FBI is rapidly approaching your location, prepare yourself";

    [Description("What should the broadcast say to the MTF & Guards that are going to be teleported to a target")]
    public string FBIOpenUpMTFText { get; set; } = "Fight or Flight.";
    [Description("How long should it take before the foundation gets teleported to the target?")]
    public float FBITeleportTime { get; set; } = 5f;

    [Description("Should the grenade feet event be active?")]
    public bool GrenadeFeetEvent { get; set; } = true;

    [Description("Should grenade fuses be random?")]
    public bool GrenadeFeetRandomFuse { get; set; } = true;

    [Description("How long should the fuse on the grenades be if it's not random?")]
    public float GrenadeFeetFuse { get; set; } = 5f;

    [Description("What should the broadcast be before the grenades drop on people?")]
    public string GrenadeFeetText { get; set; } = "You might want to be careful on where you step";

    [Description("Should unsafe medical items event be active?")]
    public bool UnsafeMedicalItemsEvent { get; set; } = true;

    [Description("What should the broadcast be for unsafe medical items?")]
    public string UnsafeMedicalItemsText { get; set; } = "Use medical items with care";

    [Description("Should unsafe medical items use a random time for the event?")]
    public bool UnsafeMedicalItemsUseRandomTime { get; set; } = true;

    [Description("If random time for the event is off, how long should Unsafe Medical Items run for?")]
    public float UnsafeMedicalItemsFixedTime { get; set; } = 30f;

    [Description("What should the broadcast be to inform players it's safe to use medical items again?")]
    public string UnsafeMedicalItemsSafeToUseText { get; set; } = "It's safe to use medical items again";
}