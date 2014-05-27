// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Views
{
    public static class ScrollViewerUtilities
    {
        public static void ScrollToProportion(ScrollViewer scrollViewer, double scrollViewerOffsetProportion)
        {
            // Update the Horizontal and Vertical offset
            if (scrollViewer == null) return;
            var scrollViewerHorizontalOffset = scrollViewerOffsetProportion * scrollViewer.ScrollableWidth;
            var scrollViewerVerticalOffset = scrollViewerOffsetProportion * scrollViewer.ScrollableHeight;

            scrollViewer.ChangeView(scrollViewerHorizontalOffset, scrollViewerVerticalOffset, null);
        }

        public static double GetScrollViewerOffsetProportion(ScrollViewer scrollViewer)
        {
            if (scrollViewer == null) return 0;

            var horizontalOffsetProportion = (scrollViewer.ScrollableWidth == 0) ? 0 : (scrollViewer.HorizontalOffset / scrollViewer.ScrollableWidth);
            var verticalOffsetProportion = (scrollViewer.ScrollableHeight == 0) ? 0 : (scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight);

            var scrollViewerOffsetProportion = Math.Max(horizontalOffsetProportion, verticalOffsetProportion);
            return scrollViewerOffsetProportion;
        }
    }
}
