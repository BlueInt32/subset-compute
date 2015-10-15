namespace Subset
{
    public class ProductRefDTO : IPartiallyVisible
    {
        public int Id { get; set; }
        
        [HiddenWhen(VisibilityLevel.Geo, VisibilityLevel.None)]
        public string Name { get; set; }

        [VisibilityByPass, HiddenWhen(VisibilityLevel.None)]
        public FacilityDTO OtherProperty { get; set; }

        [HiddenWhen(VisibilityLevel.Geo, VisibilityLevel.None)]
        public CompanylDTO SubTypical { get; set; }

        public int Transparency { get; set; }
    }
}
