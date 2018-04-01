/**************************************************************
 *  Filename:    InjectionConfigurationReader.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Xml;

namespace LWJ.Injection.Configuration
{
    //    internal class InjectionConfigurationReader : IDisposable
    //    {
    //        private XmlDocument doc;
    //        public InjectionConfigurationReader(string xml)
    //        {
    //            if (xml == null) throw new ArgumentNullException(nameof(xml));
    //            doc = new XmlDocument();
    //            doc.LoadXml(xml);
    //        }

    //        public InjectionConfigurationReader(System.IO.Stream stream)
    //        {
    //            if (stream == null) throw new ArgumentNullException(nameof(stream));
    //            doc = new XmlDocument();
    //            doc.Load(stream);
    //        }

    //        public void Read(IInjector injector)
    //        {
    //            XmlNode injectorNode = doc.SelectSingleNode("injector");
    //            if (injectorNode == null)
    //                return;
    //            XmlNode node;
    //            node = injectorNode.SelectSingleNode("callPolicies");
    //            if (node != null)
    //            {
    //                foreach (XmlNode callPolicyNode in node.SelectNodes("callPolicy"))
    //                {
    //                    ParseCallPolicyNode(callPolicyNode, injector);
    //                }
    //            }
    //        }


    //        private void ParseCallPolicyNode(XmlNode callPolicyNode, IInjector injector)
    //        {
    //            string name = callPolicyNode.GetAttributeValueOrDefault<string>("name", null);
    //            var callPolicy = injector.AddCallPolicy(name);
    //            XmlNode node;

    //            node = callPolicyNode.SelectSingleNode("behaviours");
    //            if (node != null)
    //            {
    //                foreach (XmlNode behaviourNode in node.SelectNodes("behaviour"))
    //                    ParseBehaviourNode(behaviourNode, callPolicy);
    //            }

    //            node = callPolicyNode.SelectSingleNode("matchRules");
    //            if (node != null)
    //            {
    //                foreach (XmlNode matchRuleNode in node.SelectNodes("matchRule"))
    //                    ParseMatchRuleNode(matchRuleNode, callPolicy);
    //            }

    //        }

    //        private void ParseBehaviourNode(XmlNode behaviourNode, ICallPolicy callPolicy)
    //        {
    //            Type type;
    //            InjectConstructor constructor;
    //            IBuilderValue[] properties;
    //            ParseClassNode(behaviourNode, out type, out constructor, out properties);

    //            callPolicy.AddBehaviour(type, constructor, properties);
    //        }
    //        private void ParseClassNode(XmlNode classNode, out Type type, out InjectConstructor constructor, out IBuilderValue[] properties)
    //        {
    //            object[] parameters;
    //            ParseClassNode(classNode, out type, out parameters, out properties);
    //            if (parameters == null)
    //                constructor = null;
    //            else
    //                constructor = new InjectConstructor(parameters);
    //        }
    //        private void ParseClassNode(XmlNode classNode, out Type type, out object[] constructorParameters, out IBuilderValue[] properties)
    //        {
    //            type = classNode.GetAttributeValue<Type>("type");
    //            var parametersNode = classNode.SelectSingleNode("_constructor");
    //            if (parametersNode == null)
    //            {
    //                constructorParameters = null;
    //            }
    //            else
    //            {
    //                constructorParameters = ParseTypedArray(parametersNode);
    //            }

    //            properties = parsePropertiesNodes(classNode);
    //        }

    //        private IBuilderValue[] parsePropertiesNodes(XmlNode propertiesNode)
    //        {
    //            List<IBuilderValue> values = null;
    //            string typeName;
    //            object value;
    //            string name;
    //            foreach (XmlNode node in propertiesNode.ChildNodes)
    //            {
    //                if (values == null)
    //                    values = new List<IBuilderValue>();
    //                name = node.LocalName;
    //                bool skip = false;
    //                switch (name)
    //                {
    //                    case "_constructor_":
    //                        skip = true;
    //                        break;
    //                }
    //                if (skip)
    //                    continue;
    //                typeName = node.GetAttributeValueOrDefault<string>("type", null);
    //                if (string.IsNullOrEmpty(typeName))
    //                    value = node.InnerText;
    //                else
    //                    value = propertyXmlSerializer.ReadValue(node);
    //                IBuilderValue v = new NamedValue(name, value);
    //                values.Add(v);
    //            }
    //            if (values != null)
    //                return values.ToArray();
    //            return null;
    //        }

    //        private LWJ.Serialization.TypedXml.TypedXmlSerializer propertyXmlSerializer = new Serialization.TypedXml.TypedXmlSerializer(new Serialization.TypedXml.XmlSerializationOptions(true));
    //        private LWJ.Serialization.TypedXml.TypedXmlSerializer typedXmlSerializer = new Serialization.TypedXml.TypedXmlSerializer(new Serialization.TypedXml.XmlSerializationOptions(false));

    //        private void ParseMatchRuleNode(XmlNode matchRuleNode, ICallPolicy callPolicy)
    //        {
    //            Type type;
    //            InjectConstructor constructor;
    //            IBuilderValue[] properties;

    //            ParseClassNode(matchRuleNode, out type, out constructor, out properties);
    //            callPolicy.AddMachRule(type, constructor);
    //        }

    //        private object[] ParseTypedArray(XmlNode valuesNode)
    //        {
    //            var childNodes = valuesNode.ChildNodes;
    //            int count = childNodes.Count;
    //            if (count == 0)
    //                return InternalExtensions.EmptyObjects;
    //            object[] values = new object[count];
    //            int i = 0;

    //            foreach (XmlNode valueNode in childNodes)
    //                values[i++] = typedXmlSerializer.ReadValue(valueNode);
    //            return values;
    //        }

    //        public void Dispose()
    //        {

    //        }
    //    }

}
