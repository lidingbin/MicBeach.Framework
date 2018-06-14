using MicBeach.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Controllers.Base
{
    public class WebBaseController : BaseController<AuthenticationUser<long>>
    {
    }
}