var target = Argument("target", "Default");

Task("Default")
  .Does(() =>
{
  Information("");
  Information("██╗    ██╗███████╗ █████╗ ██╗   ██╗███████╗");
  Information("██║    ██║██╔════╝██╔══██╗██║   ██║██╔════╝");
  Information("██║ █╗ ██║█████╗  ███████║██║   ██║█████╗  ");
  Information("██║███╗██║██╔══╝  ██╔══██║╚██╗ ██╔╝██╔══╝  ");
  Information("╚███╔███╔╝███████╗██║  ██║ ╚████╔╝ ███████╗");
  Information(" ╚══╝╚══╝ ╚══════╝╚═╝  ╚═╝  ╚═══╝  ╚══════╝");
  Information("");

});

RunTarget(target);
