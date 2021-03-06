﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using naru.xml;

namespace CHaMPWorkbench.CHaMPData
{
    public class SiteBasic : naru.db.EditableNamedObject
    {
        public Watershed Watershed { get; internal set; }
        public String UTMZone { get; internal set; }

        public SiteBasic(long nSiteID, string sSiteName, long nWatershedID, string sWatershedName, string sUTMZone, naru.db.DBState eState)
            : base(nSiteID, sSiteName, eState)
        {
            Watershed = new Watershed(nWatershedID, sWatershedName, naru.db.DBState.Unchanged);
            UTMZone = sUTMZone;
        }

        public SiteBasic(SiteBasic aSite, naru.db.DBState eState)
            : base(aSite.ID, aSite.Name, eState)
        {
            Watershed = new Watershed(aSite.Watershed, naru.db.DBState.Unchanged);
            UTMZone = aSite.UTMZone;
        }

        public XmlNode CreateXMLNode(ref XmlDocument xmlDoc)
        {
            XmlNode nodSite = xmlDoc.CreateElement("site");

            XMLHelpers.AddNode(ref xmlDoc, ref nodSite, "name", this.ToString());
            XMLHelpers.AddNode(ref xmlDoc, ref nodSite, "utm_zone", UTMZone);
            XMLHelpers.AddNode(ref xmlDoc, ref nodSite, "watershed", Watershed.Name);
            XMLHelpers.AddNode(ref xmlDoc, ref nodSite, "stream_name", string.Empty);
            XMLHelpers.AddNode(ref xmlDoc, ref nodSite, "sitegdb", string.Empty);

            // Do not add the visits here. They will be handled by the batch builder.
            // Whether visits are included and their properties depend on the batch builder config.

            return nodSite;
        }
    }
}
