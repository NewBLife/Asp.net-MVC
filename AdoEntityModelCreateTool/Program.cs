using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            args = new string[] { "Model1.edmx" };

            if (args.Length < 1) return;

            string filePath = args[0];

            string entityName = null;

            // Optionally do not replace underscores which 
            // helps with naming collisions with siimilarly named
            // columns on some database tables.

            bool replaceUnderscores = true;

            // Allow for the replacement of the object context class name, which is useful
            // where multiple databases have edmx files.

            bool doEntityNameReplace = false;

            //if (args.Length > 1)
            //{
            //    entityName = args[1];
            //    doEntityNameReplace = true;
            //}

            //if (args.Length > 2)
            //{
            //    replaceUnderscores = args[2] != "0";
            //}

            if (!File.Exists(filePath))
            {
                StopWithMessage("Could not find specified file.");
                return;
            }
            if (Path.GetExtension(filePath) != ".edmx")
            {
                StopWithMessage("This works only on EDMX files.");
                return;
            }

            // Processing:

            Console.WriteLine("Creating backup: " + Path.ChangeExtension(filePath, ".bak"));
            File.Copy(filePath, Path.ChangeExtension(filePath, ".baks"), true);

            Console.WriteLine("Reading target document...");

            XDocument xdoc = XDocument.Load(filePath);

            const string SSDLNamespace = "http://schemas.microsoft.com/ado/2009/11/edm/ssdl";
            const string CSDLNamespace = "http://schemas.microsoft.com/ado/2009/11/edm";
            const string MSLNamespace = "http://schemas.microsoft.com/ado/2009/11/mapping/cs";
            const string DiagramNamespace = "http://schemas.microsoft.com/ado/2009/11/edmx";
            const string CSNameSpace = "http://schemas.microsoft.com/ado/2009/11/mapping/cs";

            XElement ssdl = xdoc.Descendants(XName.Get("Schema", SSDLNamespace)).First();
            XElement csdl = xdoc.Descendants(XName.Get("Schema", CSDLNamespace)).First();
            XElement msl = xdoc.Descendants(XName.Get("Mapping", MSLNamespace)).First();
            XElement designerDiagram = xdoc.Descendants(XName.Get("Diagram", DiagramNamespace)).FirstOrDefault();

            //modifications for renaming everything, not just table names:

            #region ssdl

            Console.WriteLine("Modifying ssdl...");
            Console.WriteLine(" - modifying entity sets...");

            foreach (var entitySet in ssdl.Element(XName.Get("EntityContainer", SSDLNamespace)).Elements(XName.Get("EntitySet", SSDLNamespace)))
            {
                entitySet.Attribute("Name").Value = FormatString(entitySet.Attribute("Name").Value, replaceUnderscores);
                entitySet.Attribute("EntityType").Value = FormatString(entitySet.Attribute("EntityType").Value, replaceUnderscores);
            }

            Console.WriteLine(" - modifying entity types...");
            foreach (var entityType in ssdl.Elements(XName.Get("EntityType", SSDLNamespace)))
            {
                entityType.Attribute("Name").Value = FormatString(entityType.Attribute("Name").Value, replaceUnderscores);

                foreach (var key in entityType.Elements(XName.Get("Key", SSDLNamespace)))
                {
                    foreach (var propertyRef in key.Elements(XName.Get("PropertyRef", SSDLNamespace)))
                    {
                        propertyRef.Attribute("Name").Value = FormatString(propertyRef.Attribute("Name").Value, replaceUnderscores);
                    }
                }

                foreach (var property in entityType.Elements(XName.Get("Property", SSDLNamespace)))
                {
                    property.Attribute("Name").Value = FormatString(property.Attribute("Name").Value, replaceUnderscores);
                }
            }

            #endregion


            #region CSDL2

            Console.WriteLine("Modifying CSDL...");
            Console.WriteLine(" - modifying entity sets...");

            foreach (var entitySet in csdl.Element(XName.Get("EntityContainer", CSDLNamespace)).Elements(XName.Get("EntitySet", CSDLNamespace)))
            {
                entitySet.Attribute("Name").Value = FormatString(entitySet.Attribute("Name").Value, replaceUnderscores);
                entitySet.Attribute("EntityType").Value = FormatString(entitySet.Attribute("EntityType").Value, replaceUnderscores);
            }

            Console.WriteLine(" - modifying association sets...");
            foreach (var associationSet in csdl.Element(XName.Get("EntityContainer", CSDLNamespace)).Elements(XName.Get("AssociationSet", CSDLNamespace)))
            {
                foreach (var end in associationSet.Elements(XName.Get("End", CSDLNamespace)))
                {
                    end.Attribute("EntitySet").Value = FormatString(end.Attribute("EntitySet").Value, replaceUnderscores);
                }
            }

            Console.WriteLine(" - modifying entity types...");
            foreach (var entityType in csdl.Elements(XName.Get("EntityType", CSDLNamespace)))
            {
                entityType.Attribute("Name").Value = FormatString(entityType.Attribute("Name").Value, replaceUnderscores);

                foreach (var key in entityType.Elements(XName.Get("Key", CSDLNamespace)))
                {
                    foreach (var propertyRef in key.Elements(XName.Get("PropertyRef", CSDLNamespace)))
                    {
                        propertyRef.Attribute("Name").Value = FormatString(propertyRef.Attribute("Name").Value, replaceUnderscores);
                    }
                }

                foreach (var property in entityType.Elements(XName.Get("Property", CSDLNamespace)))
                {
                    property.Attribute("Name").Value = FormatString(property.Attribute("Name").Value, replaceUnderscores);
                }

                foreach (var navigationProperty in entityType.Elements(XName.Get("NavigationProperty", CSDLNamespace)))
                {
                    navigationProperty.Attribute("Name").Value = FormatString(navigationProperty.Attribute("Name").Value, replaceUnderscores);
                }

            }

            Console.WriteLine(" - modifying associations...");
            foreach (var association in csdl.Elements(XName.Get("Association", CSDLNamespace)))
            {
                foreach (var end in association.Elements(XName.Get("End", CSDLNamespace)))
                {
                    end.Attribute("Type").Value = FormatString(end.Attribute("Type").Value, replaceUnderscores);
                }
                foreach (var propref in association.Descendants(XName.Get("PropertyRef", CSDLNamespace)))
                {
                    //propertyrefs are contained in constraints
                    propref.Attribute("Name").Value = FormatString(propref.Attribute("Name").Value, replaceUnderscores);
                }
            }

            #endregion

            #region MSL2

            Console.WriteLine("Modifying MSL...");
            Console.WriteLine(" - modifying entity set mappings...");

            foreach (var entitySetMapping in msl.Element(XName.Get("EntityContainerMapping", MSLNamespace)).Elements(XName.Get("EntitySetMapping", MSLNamespace)))
            {
                entitySetMapping.Attribute("Name").Value = FormatString(entitySetMapping.Attribute("Name").Value, replaceUnderscores);

                foreach (var entityTypeMapping in entitySetMapping.Elements(XName.Get("EntityTypeMapping", MSLNamespace)))
                {
                    entityTypeMapping.Attribute("TypeName").Value = FormatString(entityTypeMapping.Attribute("TypeName").Value, replaceUnderscores);
                    foreach
                    (var scalarProperty in
                    (entityTypeMapping.Element(XName.Get("MappingFragment", MSLNamespace))).Elements(XName.Get("ScalarProperty", MSLNamespace))
                    )
                    {
                        scalarProperty.Attribute("Name").Value = FormatString(scalarProperty.Attribute("Name").Value, replaceUnderscores);
                    }
                }
            }

            Console.WriteLine(" - modifying association set mappings...");

            foreach (var associationSetMapping in msl.Element(XName.Get("EntityContainerMapping", MSLNamespace)).Elements(XName.Get("AssociationSetMapping", MSLNamespace)))
            {
                foreach (var endProperty in associationSetMapping.Elements(XName.Get("EndProperty", MSLNamespace)))
                {
                    foreach (var scalarProperty in endProperty.Elements(XName.Get("ScalarProperty", MSLNamespace)))
                    {
                        scalarProperty.Attribute("Name").Value = FormatString(scalarProperty.Attribute("Name").Value, replaceUnderscores);
                    }
                }
            }
            #endregion


            #region Designer

            //Console.WriteLine("Modifying designer content...");
            //foreach (var item in designerDiagram.Elements(XName.Get("EntityTypeShape", DiagramNamespace)))
            //{
            //    item.Attribute("EntityType").Value = FormatString(item.Attribute("EntityType").Value, replaceUnderscores);
            //}

            #endregion

            // Optionally replace the entity name in case the default of "Entity" is not
            // sufficient for your needs.

            if (doEntityNameReplace)
            {
                Console.WriteLine("Modifying entity name refs...");

                // CSDL
                xdoc.Descendants(XName.Get("EntityContainer", CSDLNamespace)).First().Attribute("Name").Value = entityName;

                // Diagram 
                xdoc.Descendants(XName.Get("Diagram", DiagramNamespace)).First().Attribute("Name").Value = entityName;

                // Diagram 
                xdoc.Descendants(XName.Get("EntityContainerMapping", CSNameSpace)).First().Attribute("CdmEntityContainer").Value = entityName;
            }

            Console.WriteLine("Writing result...");

            using (XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.Default))
            {
                writer.Formatting = Formatting.Indented;
                xdoc.WriteTo(writer);
            }
        }

        /// <summary>
        /// Formats the string to pascal case, additionally checking for a period
        /// in the string (in which case it skips past the period, which indicates 
        /// the use of namespace in a string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="replaceUnderscores"></param>
        /// <returns></returns>
        private static string FormatString(string str, bool replaceUnderscores = true)
        {
            if (str.EndsWith("s"))
            {
                str = str.Substring(0, str.Length - 1);
            }

            if (str.ToUpper().StartsWith("T_"))
            {
                str = str.Substring(2) + "Model";
            }
            else if (str.ToUpper().StartsWith("SELF.T_"))
            {
                str = str.Replace("Self.t_", "Self.") + "Model";
            }

            char[] chars = str.ToCharArray();

            var sb = new StringBuilder();

            bool previousCharWasUpper = false;
            bool lastOperationWasToLower = false;

            int startPos = 0;

            if (str.Contains("."))
            {
                if (str.IndexOf(".") < (str.Length - 1))
                {
                    startPos = str.IndexOf(".") + 1;
                }

                sb.Append(str.Substring(0, startPos));
            }

            for (int i = startPos; i < chars.Length; i++)
            {
                char character = chars[i];

                if (Char.IsLetter(character))
                {
                    if (Char.IsLower(character))
                    {
                        bool toUpper = false;

                        if (i > 0)
                        {
                            // Look at the previous char to see if not a letter

                            if (!Char.IsLetter(chars[i - 1]))
                            {
                                toUpper = true;
                            }
                        }

                        if (i == 0 || toUpper)
                        {
                            character = Char.ToUpper(character);

                            lastOperationWasToLower = false;
                        }
                    }
                    else // IsUpper = true
                    {
                        if (previousCharWasUpper || lastOperationWasToLower)
                        {
                            character = Char.ToLower(character);

                            lastOperationWasToLower = true;
                        }
                    }

                    previousCharWasUpper = Char.IsUpper(character);

                    sb.Append(character);
                }
                else
                {
                    if (Char.IsDigit(character))
                    {
                        sb.Append(character);

                        previousCharWasUpper = false;
                        lastOperationWasToLower = false;
                    }
                    else if (!replaceUnderscores)
                    {
                        if (character == '_')
                        {
                            sb.Append(character);
                        }
                    }
                }
            }

            return sb.ToString();

        }

        private static void StopWithMessage(string str)
        {
            Console.WriteLine(str);

            Console.ReadLine();

            throw new InvalidOperationException("Cannot continue.");
        }
    }
}
