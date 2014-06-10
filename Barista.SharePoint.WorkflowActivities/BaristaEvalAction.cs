﻿namespace Barista.SharePoint.WorkflowActivities
{
    using System.Globalization;
    using Barista.SharePoint.Services;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Workflow;
    using Microsoft.SharePoint.WorkflowActions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;

    // ReSharper disable InconsistentNaming
    public class BaristaEvalAction : Activity
    {
        public static DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(BaristaEvalAction));

        public WorkflowContext __Context
        {
            get
            {
                return ((WorkflowContext)(GetValue(BaristaEvalAction.__ContextProperty)));
            }
            set
            {
                SetValue(BaristaEvalAction.__ContextProperty, value);
            }
        }

        public static DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(BaristaEvalAction));

        public string __ListId
        {
            get
            {
                return ((string)(GetValue(BaristaEvalAction.__ListIdProperty)));
            }
            set
            {
                SetValue(BaristaEvalAction.__ListIdProperty, value);
            }
        }

        public static DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(SPItemKey), typeof(BaristaEvalAction));

        public SPItemKey __ListItem
        {
            get
            {
                return ((SPItemKey)(GetValue(__ListItemProperty)));
            }
            set
            {
                SetValue(__ListItemProperty, value);
            }
        }

        public static DependencyProperty __ActivationPropertiesProperty = DependencyProperty.Register("__ActivationProperties", typeof(SPWorkflowActivationProperties), typeof(BaristaEvalAction));

        public SPWorkflowActivationProperties __ActivationProperties
        {
            get
            {
                return (SPWorkflowActivationProperties)GetValue(BaristaEvalAction.__ActivationPropertiesProperty);
            }
            set
            {
                SetValue(BaristaEvalAction.__ActivationPropertiesProperty, value);
            }
        }

        public static DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(BaristaEvalAction));
        [Category("Barista"), Browsable(true)]
        [DesignerSerializationVisibility
          (DesignerSerializationVisibility.Visible)]
        public string Code
        {
            get
            {
                return Convert.ToString(GetValue(CodeProperty));
            }
            set
            {
                SetValue(CodeProperty, value);
            }
        }

        public static DependencyProperty BodyProperty = DependencyProperty.Register("Body", typeof(string), typeof(BaristaEvalAction));
        [Category("Barista"), Browsable(true)]
        [DesignerSerializationVisibility
          (DesignerSerializationVisibility.Visible)]
        public string Body
        {
            get
            {
                return Convert.ToString(GetValue(BodyProperty));
            }
            set
            {
                SetValue(BodyProperty, value);
            }
        }

        public static DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(string), typeof(BaristaEvalAction));
        [Category("Barista"), Browsable(true)]
        [DesignerSerializationVisibility
          (DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Optional)]
        public string Result
        {
            get
            {
                return Convert.ToString(GetValue(ResultProperty));
            }
            set
            {
                SetValue(ResultProperty, value);
            }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Result = String.Empty;

            // get all of the information we currently have about the item
            // that this workflow is running on
            var listGuid = new Guid(__ListId);
            var web = __Context.Web;
            var myList = __Context.Web.Lists[listGuid];
            var myItem = __Context.GetListItem(myList, __ListItem);

            var serviceContext = SPServiceContext.GetContext(__Context.Site);
            var client = new BaristaServiceClient(serviceContext);

            var request = new BrewRequest
            {
                ContentType = "application/json", //default to application/json.
                Code = Code,
                Body = Encoding.UTF8.GetBytes(Body),
                ExtendedProperties = new Dictionary<string, string> {
                    {"SPSiteId", __Context.Site.ID.ToString()},
                    {"SPUrlZone", __Context.Site.Zone.ToString()},
                    {"SPUserToken", Convert.ToBase64String(__Context.Site.UserToken.BinaryToken)},
                    {"SPWebId", web.ID.ToString()},
                    {"SPListId", __ListId},
                    {"SPListItemUrl", myItem.Url},
                    {"SPWorkflowAssociationTitle", __Context.AssociationTitle},
                    {"SPWorkflowInstanceId", __Context.WorkflowInstanceId.ToString()},
                    {"SPWorkflowCurrentItemUrl", __Context.CurrentItemUrl},
                    {"SPWorkflowCurrentWebUrl", __Context.CurrentWebUrl},
                    {"SPWorkflowItemName", __Context.ItemName},
                    {"SPWorkflowTaskListGuid", __Context.TaskListGuid},
                    {"SPWorkflowStatusUrl", __Context.WorkflowStatusUrl},
                    {"SPWorkflowAssociatorUserLoginName", __Context.AssociatorUser.LoginName},
                    {"SPWorkflowInitiatorUserLoginName", __Context.InitiatorUser.LoginName},
                    {"SPWorkflowItemId", __Context.ItemId.ToString(CultureInfo.InvariantCulture)},
                    {"SPWorkflowStartedDateTime", __Context.StartedDateTime.ToUniversalTime().ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz")},
                    {"SPWorkflowLastRunDateTime", __Context.LastRunDateTime.ToUniversalTime().ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz")}
                }
            };

            var response = client.Eval(request);
            Result = Encoding.UTF8.GetString(response.Content);

            return ActivityExecutionStatus.Closed;
        }
    }
    // ReSharper restore InconsistentNaming
}
