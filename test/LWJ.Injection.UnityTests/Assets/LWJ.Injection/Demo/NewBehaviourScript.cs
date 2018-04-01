using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWJ.Injection;
using System.Xml;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //var assembly = typeof(Injector).Assembly;
        //using (var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".injector.xml"))
        //{
        //	XmlDocument doc = new XmlDocument();
        //	doc.Load(stream);
        //          Debug.Log(doc.OuterXml);

        //}
        throw new LWJ.Injection.Aop.Permission.InvalidOperationPermissionException("AA");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
