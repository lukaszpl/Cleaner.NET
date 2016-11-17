﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Cleaner.NET
{
    public class WindowsItems
    {
        public ObservableCollection<object> GetWindowsElements()
        {
            ObservableCollection<object> ObjList = new ObservableCollection<object>();
            ObjList.Add(GetWindowsTextBlock());
            ObjList.Add(GetCleanTempFilesCheckBox());
            ObjList.Add(GetCleanWinErrorsCheckBox());
            ObjList.Add(GetCleanFontCacheCheckBox());
            ObjList.Add(GetCleanCacheDNSCheckBox());
            ObjList.Add(GetCleanClipboradCheckBox());
            ObjList.Add(GetCleanTrashCheckBox());
            ObjList.Add(GetWindowsExplorerTextBlock());
            ObjList.Add(GetCleanRecentDocumentsCheckBox());
            ObjList.Add(GetCleanThubCacheCheckBox());
            return ObjList;
        }
        private object GetWindowsTextBlock()
        {
            TextBlock tb = new TextBlock();
            BitmapImage MyImageSource = new BitmapImage(new Uri("pack://application:,,,/Cleaner.NET;component/Resources/logowindows.png", UriKind.RelativeOrAbsolute));

            Image image = new Image();
            image.Source = MyImageSource;
            image.Width = 24;
            image.Height = 24;
            image.Visibility = Visibility.Visible;

            InlineUIContainer container = new InlineUIContainer(image);
            tb.Inlines.Add(container);

            var textRem = new Run("Windows:");
            tb.Inlines.Add(textRem);
            return tb;
        }
        private object GetWindowsExplorerTextBlock()
        {
            TextBlock tb = new TextBlock();
            BitmapImage MyImageSource = new BitmapImage(new Uri("pack://application:,,,/Cleaner.NET;component/Resources/explorerwin.png", UriKind.RelativeOrAbsolute));

            Image image = new Image();
            image.Source = MyImageSource;
            image.Width = 24;
            image.Height = 24;
            image.Visibility = Visibility.Visible;

            InlineUIContainer container = new InlineUIContainer(image);
            tb.Inlines.Add(container);

            var textRem = new Run("Windows Explorer:");
            tb.Inlines.Add(textRem);
            return tb;
        }
        private object GetCleanTempFilesCheckBox()
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Name = "CleanTempFiles";
            checkBox.FontWeight = FontWeights.Normal;
            checkBox.Content = Languages.Lang.Temp_CheckBox;
            return checkBox;
        }
        private object GetCleanWinErrorsCheckBox()
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Name = "CleanWinErrors";
            checkBox.FontWeight = FontWeights.Normal;
            checkBox.Content = Languages.Lang.WinErrors_CheckBox;
            return checkBox;
        }
        private object GetCleanFontCacheCheckBox()
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Name = "CleanFontCache";
            checkBox.FontWeight = FontWeights.Normal;
            checkBox.Content = Languages.Lang.TempFont_CheckBox;
            return checkBox;
        }
        private object GetCleanCacheDNSCheckBox()
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Name = "CleanCacheDNS";
            checkBox.FontWeight = FontWeights.Normal;
            checkBox.Content = Languages.Lang.TempDns_CheckBox;
            return checkBox;
        }
        private object GetCleanClipboradCheckBox()
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Name = "CleanClipboard";
            checkBox.FontWeight = FontWeights.Normal;
            checkBox.Content = Languages.Lang.Clipboard_CheckBox;
            return checkBox;
        }
        private object GetCleanTrashCheckBox()
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Name = "CleanTrash";
            checkBox.FontWeight = FontWeights.Normal;
            checkBox.Content = Languages.Lang.RecycleBin_CheckBox;
            return checkBox;
        }
        private object GetCleanRecentDocumentsCheckBox()
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Name = "CleanRecentDocuments";
            checkBox.FontWeight = FontWeights.Normal;
            checkBox.Content = Languages.Lang.Recentdoc_CheckBox;
            return checkBox;
        }
        private object GetCleanThubCacheCheckBox()
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Name = "CleanThubCache";
            checkBox.FontWeight = FontWeights.Normal;
            checkBox.Content = Languages.Lang.ThumbnailsCache_CheckBox;
            return checkBox;
        }
    }
}
