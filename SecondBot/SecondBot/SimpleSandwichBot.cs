using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace SecondBot.SimpleSandwichBot
{
    public enum SandwichOptions
    {
        BLT, BlackForestHam, BuffaloChicken, ChickenAndBaconRanchMelt, ColdCutCombo, MeatballMarinara,
        OverRoastedChicken, RoastBeef,
        [Terms(@"rotis\w* style chicken", MaxPhrase = 3)]
        RotisserieStyleChicken,
        SpicyItalian, SteakAndCheese, SweetOnionTeriyaki, Tuna,
        TurkeyBreast, Veggie
    };
    public enum LengthOptions { SixInch, FootLong};
    public enum BreadOptions { NineGrainWheat, NineGrainHoneyOat, Italian, ItalianHerbsAndCheese, Flatbread };
    public enum CheeseOptions { American, MontereyCheddar, Pepperjack };
    public enum ToppingOptions
    {
        [Terms("except", "but", "not", "all", "everything")]
        Everything = 1,
        Avocado, BananaPeppers, Cucumbers, GreenBellPeppers, Jalapenos,
        Lettuce, Olives, Pickles, RedOnion, Spinach, Tomatoes
    };
    public enum SauceOptions
    {
        ChipotleSouthwest, HoneyMustard, LightMayonnaise, RegularMayonnaise,
        Mustard, Oil, Pepper, Ranch, SweetOnion, Vinegar
    };

    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "Ich verstehe \"{0}\" nicht.", "Versuche es nochmal, ich verstehe \"{0}\" nicht.")]
    [Template(TemplateUsage.EnumSelectOne, "Welche Art von {&} möchtest du auf dein Sandwich?", ChoiceStyle = ChoiceStyleOptions.PerLine)]
    class SandwichOrder
    {
        [Prompt("Welche Art von {&} möchtest du? {||}", ChoiceFormat = "{1}")]
        public SandwichOptions? Sandwich;
        public LengthOptions? Length;
        public BreadOptions? Bread;

        [Optional]
        public CheeseOptions? Cheese;
        public List<ToppingOptions> Toppings
        {
            get { return _toppings; }
            set
            {
                if (value.Contains(ToppingOptions.Everything))
                {
                    _toppings = (from ToppingOptions topping in Enum.GetValues(typeof(ToppingOptions))
                                 where topping != ToppingOptions.Everything && !value.Contains(topping)
                                 select topping).ToList();
                }
                else
                {
                    _toppings = value;
                }
            }
        }
        private List<ToppingOptions> _toppings;
        public List<SauceOptions> Sauce;

        public static IForm<SandwichOrder> BuildForm()
        {
            return new FormBuilder<SandwichOrder>().Message("Welcome to the simple sandwich order bot!").Build();
        }
    }
}