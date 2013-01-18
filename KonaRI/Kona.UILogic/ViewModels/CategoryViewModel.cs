// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using Kona.UILogic.Models;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Kona.UILogic.ViewModels
{
    public class CategoryViewModel
    {
        private readonly Category _category;
        private ImageSource _image;
        private List<CategoryViewModel> _subCategoryViewModels;

        public CategoryViewModel(Category category)
        {
            _category = category;
            _subCategoryViewModels = new List<CategoryViewModel>();
            if (category != null && category.Subcategories != null)
            {
                foreach (var subCategory in category.Subcategories)
                {
                    _subCategoryViewModels.Add(new CategoryViewModel(subCategory));
                }
                Subcategories = _subCategoryViewModels;
            }

        }

        public int CategoryId { get { return _category.Id; } }

        public int ParentCategoryId { get { return _category.ParentId; } }

        public string Title { get { return _category.Title; } }

        public IEnumerable<CategoryViewModel> Subcategories { get; private set; } 

        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._category.ImageLocalUri != null)
                {
                    this._image = new BitmapImage(this._category.ImageLocalUri);
                }
                return this._image;
            }
        }
    }
}
