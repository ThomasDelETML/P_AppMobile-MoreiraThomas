﻿using ReadME;

namespace ReadME
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            Routing.RegisterRoute(nameof(LirePage), typeof(LirePage));
        }
    }
}
