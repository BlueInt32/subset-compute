using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Subset
{
    class Program
    {
        static void Main(string[] args)
        {
            ComputeVisibilityMatrix();
        }

        //private List<bool> 

        List<string> KeptPropertiesForProductRef { get; set; }

        private static JObject ComputeVisibilityMatrix()
        {


            var visibleTypes = TypesImplementingInterface(typeof(IPartiallyVisible));
            var booleanValues = new List<bool> { false, true };
            var visibilityLevels = new List<VisibilityLevel> { VisibilityLevel.None, VisibilityLevel.Geo, VisibilityLevel.Full };


            foreach (Type type in visibleTypes)
            {
                var propertiesInType = type.GetProperties();
                var outputMatrix = BuildMatrix();
                
                foreach (bool bypassValue in booleanValues)
                {
                    foreach (VisibilityLevel visibilityLevel in visibilityLevels)
                    {
                        List<string> visibleProperties = new List<string>();
                        foreach (PropertyInfo propertyInfo in propertiesInType)
                        {
                            if (ComputePropertyVisibility(propertyInfo, bypassValue, visibilityLevel))
                                visibleProperties.Add(propertyInfo.Name);
                        }
                        outputMatrix[bypassValue][visibilityLevel] = visibleProperties;
                    }
                }
                Console.Write(outputMatrix.Show());
            }

            Console.Read();
            return new JObject();
        }

        private static Dictionary<bool, Dictionary<VisibilityLevel, List<string>>> BuildMatrix()
        {
            var output = new Dictionary<bool, Dictionary<VisibilityLevel, List<string>>>();
            output[false] = new Dictionary<VisibilityLevel, List<string>>();
            output[true] = new Dictionary<VisibilityLevel, List<string>>();

            return output;
        }

        private static bool ComputePropertyVisibility(PropertyInfo propertyInfo, bool bypassParam, VisibilityLevel visibilityLevel)
        {
            bool isVisible = false;
            var hiddenAttribute = propertyInfo.GetCustomAttribute(typeof(HiddenWhenAttribute));
            var visibilityByPassAttribute = propertyInfo.GetCustomAttribute(typeof(VisibilityByPassAttribute));

            if (visibilityByPassAttribute == null && hiddenAttribute == null) // no attribute = keep in any case
            {
                isVisible = true;
            }
            else if (visibilityByPassAttribute != null && bypassParam)  // bypass attribute + bypassParam = keep
            {
                isVisible = true;
            }
            else if (hiddenAttribute != null)
            {
                HiddenWhenAttribute hiddenLevels = hiddenAttribute as HiddenWhenAttribute;
                if (hiddenLevels != null)
                {
                    if (!hiddenLevels.Levels.Contains(visibilityLevel))
                        isVisible = true;
                }
            }
            else
            {
                isVisible = false;
            }
            return isVisible;
        }

        private static IEnumerable<Type> TypesImplementingInterface(Type desiredType)
        {
            return AppDomain
                   .CurrentDomain
                   .GetAssemblies()
                   .SelectMany(assembly => assembly.GetTypes())
                   .Where(type => desiredType.IsAssignableFrom(type) && type.IsInterface == false);
        }
    }
}
