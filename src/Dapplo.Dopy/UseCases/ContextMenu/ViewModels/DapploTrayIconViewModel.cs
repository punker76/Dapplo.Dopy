﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Dopy
// 
//  Dapplo.Dopy is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Dopy is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Dopy. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Autofac.Features.AttributeFilters;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.CaliburnMicro.NotifyIconWpf.ViewModels;
using Dapplo.Dopy.Translations;
using Dapplo.Windows.Common;
using MahApps.Metro.IconPacks;

namespace Dapplo.Dopy.UseCases.ContextMenu.ViewModels
{
    /// <summary>
    ///     Implementation of the system-tray icon
    /// </summary>
    public class DapploTrayIconViewModel : TrayIconViewModel
    {
        private readonly IEnumerable<Lazy<IMenuItem>> _contextMenuItems;
        private readonly IMainContextMenuTranslations _contextMenuTranslations;

        public DapploTrayIconViewModel(
            IMainContextMenuTranslations contextMenuTranslations,
            ITrayIconManager trayIconManager,
            [MetadataFilter("Menu", "contextmenu")]IEnumerable<Lazy<IMenuItem>> contextMenuItems = null
        ) : base(trayIconManager)
        {
            _contextMenuTranslations = contextMenuTranslations;
            _contextMenuItems = contextMenuItems;
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            // Set the title of the icon (the ToolTipText) to our IContextMenuTranslations.Title
            _contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IMainContextMenuTranslations.Title));

            var items = new List<IMenuItem>();

            // Lazy values
            items.AddRange(_contextMenuItems.Select(lazy => lazy.Value));

            items.Add(new MenuItem
            {
                Style = MenuItemStyles.Separator,
                Id = "Y_Separator"
            });
            ConfigureMenuItems(items);

            // Make sure the margin is set, do this AFTER the icon are set
            items.ApplyIconMargin(new Thickness(2, 2, 2, 2));

            SetIcon(new PackIconMaterial
            {
                Kind = PackIconMaterialKind.Clipboard,
                Background = Brushes.Transparent,
                Foreground = WindowsVersion.IsWindows8OrLater? Brushes.White : Brushes.Black
            });
            Show();
        }
    }
}