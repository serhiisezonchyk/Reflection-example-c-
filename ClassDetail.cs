using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection_l2
{
    internal class ClassDetail
    {
        public static string GetFullName(Type type)
        {
            string name = "Name:\r\n";
            name += (type.IsPublic && type.IsVisible ? "public " : "")
                       + (type.IsNested && type.IsNestedFamily ? "protected " : "")
                       + (type.IsNotPublic && !type.IsNested && !type.IsNestedPrivate && !type.IsNestedFamily ? "internal " : "")
                       + (type.IsNested && type.IsNestedPrivate ? "private " : "")
                       + (type.IsAbstract && type.IsSealed ? "static " : "");
            name += type.IsClass ? "class "
                      : type.IsInterface ? "interface "
                      : type.IsEnum ? "enum "
                      : type.IsValueType ? "struct " : "";
            name += type.FullName + "\r\n";
            return name;
        }

        public static string GetFieldsString(FieldInfo[] fields)
        {
            string fieldsString = "Fields:\r\n";

            foreach (FieldInfo field in fields)
            {
                fieldsString += field.IsPublic ? "public " : field.IsPrivate ? "private " : "protected ";
                fieldsString += field.IsStatic ? "static " : "";
                fieldsString += field.FieldType.Name + " " + field.Name + ";\r\n";
            }
            return fieldsString;
        }

        public static string GetConstructorsString(ConstructorInfo[] constructors)
        {
            string constructorsString = "Consructors:\r\n";

            foreach (ConstructorInfo constructor in constructors)
            {
                constructorsString += (constructor.IsStatic ? "static " : "")
                       + (constructor.IsPrivate ? "private " : "")
                       + (constructor.IsFamily ? "protected " : "")
                       + (constructor.IsPublic ? "public " : "")
                       + (constructor.IsAssembly ? "internal " : "")
                       + constructor.Name;
                constructorsString += "(";

                ParameterInfo[] parameters = constructor.GetParameters();
                bool firstEntry = false;
                foreach (ParameterInfo parameter in parameters)
                {
                    constructorsString += (firstEntry? ", " : "")
                          + (parameter.IsIn ? "in " : "")
                          + (parameter.IsOut ? "out " : "")
                          + (parameter.IsOptional ? "optional " : "")
                          + (parameter.IsLcid ? "lcid " : "")
                          + parameter.ParameterType.Name + " "
                          + parameter.Name
                          + ((parameter.DefaultValue != DBNull.Value)
                            ? (" = " + parameter.DefaultValue) : "");
                    firstEntry = true;
                }
                constructorsString += ")\r\n";
            }
            return constructorsString;
        }



        public static string GetMethodsString(MethodInfo[] methods)
        {
            string methodsString = "Methods:\r\n";

            foreach (MethodInfo method in methods)
            {
                methodsString += (method.IsPublic ? "public " : "")
                             + (method.IsPrivate ? "private " : "")
                             + (method.IsFamily ? "protected" : "")
                             + (method.IsAssembly ? "internal " : "")
                             + (method.IsStatic ? "static " : "")
                             + (method.IsVirtual ? "virtual " : "")
                             + (method.IsAbstract ? "abstract " : "")
                             + method.ReturnType.Name + " "
                             + method.Name;
                methodsString += "(";


                ParameterInfo[] parameters = method.GetParameters();
                bool firstEntry = false;
                foreach (ParameterInfo parameter in parameters)
                {
                    methodsString += (firstEntry ? "," : "")
                          + (parameter.IsIn ? "in " : "")
                          + (parameter.IsOut ? "out " : "")
                          + (parameter.IsOptional ? "optional " : "")
                          + (parameter.IsLcid ? "lcid " : "")
                          + parameter.ParameterType.Name + " "
                          + parameter.Name
                          + ((parameter.DefaultValue != DBNull.Value)
                            ? (" = " + parameter.DefaultValue) : "");
                    firstEntry = true;
                }
                methodsString += ")\r\n";
            }
            return methodsString;
        }
        public static string GetMemberString(MemberInfo member)
        {
            string memberString = "";

            if (member.MemberType == MemberTypes.Field)
            {
                FieldInfo field = (FieldInfo)member;

                memberString += "Fields:\r\n";
                memberString += field.IsPublic ? "public " : field.IsPrivate ? "private " : "protected ";
                memberString += field.IsStatic ? "static " : "";
                memberString += field.FieldType.Name + " " + field.Name + ";\r\n";
            }
            else if (member.MemberType == MemberTypes.Method)
            {
                MethodInfo method = (MethodInfo)member;

                memberString = "Methods:\r\n";
                memberString += (method.IsPublic ? "public " : "")
                      + (method.IsPrivate ? "private " : "")
                      + (method.IsFamily ? "protected " : "")
                      + (method.IsStatic ? "static " : "")
                      + (method.IsAbstract ? "abstract " : "")
                      + (method.IsAssembly ? "internal " : "")
                      + (method.IsVirtual ? "virtual " : "")
                      + method.ReturnType.Name + " "
                      + method.Name;
                memberString += "(";

                ParameterInfo[] parameters = method.GetParameters();
                bool firstEntry = false;
                foreach (ParameterInfo parameter in parameters)
                {
                    memberString += (firstEntry  ? ", " : "")
                          + (parameter.IsIn ? "in " : "")
                          + (parameter.IsOut ? "out " : "")
                          + (parameter.IsOptional ? "optional " : "")
                          + (parameter.IsLcid ? "lcid " : "")
                          + parameter.ParameterType.Name + " "
                          + parameter.Name
                          + ((parameter.DefaultValue != DBNull.Value)
                             ? (" = " + parameter.DefaultValue) : "");
                    firstEntry = true;
                }
                memberString += ")\r\n";
            }
            else if (member.MemberType == MemberTypes.Constructor)
            {
                ConstructorInfo constructor = (ConstructorInfo)member;

                memberString += "Constructors:\r\n";
                memberString += (constructor.IsStatic ? "static " : "")
                      + (constructor.IsPrivate ? "private " : "")
                      + (constructor.IsFamily ? "protected " : "")
                      + (constructor.IsPublic ? "public " : "")
                      + (constructor.IsAssembly ? "internal " : "")
                      + constructor.Name;
                memberString += "(";

                ParameterInfo[] parameters = constructor.GetParameters();
                bool firstEntry = false;
                foreach (ParameterInfo parameter in parameters)
                {
                    memberString += (firstEntry ? ", " : "")
                          + (parameter.IsIn ? "in " : "")
                          + (parameter.IsOut ? "out " : "")
                          + (parameter.IsOptional ? "optional " : "")
                          + (parameter.IsLcid ? "lcid " : "")
                          + parameter.ParameterType.Name + " "
                          + parameter.Name
                          + ((parameter.DefaultValue != DBNull.Value)
                            ? (" = " + parameter.DefaultValue) : "");
                    firstEntry = true;
                }
                memberString += ")\r\n";
            }

            return memberString;
        }
    }
}
