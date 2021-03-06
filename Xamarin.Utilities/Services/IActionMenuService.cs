﻿using ReactiveUI;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Xamarin.Utilities.Services
{
    public interface IActionMenuService
    {
        IActionMenu Create(string title);

        IPickerMenu CreatePicker();

        Task ShareUrl(string url);

        void SendToPasteBoard(string str);
    }

    public interface IActionMenu
    {
        void AddButton(string title, IReactiveCommand clickAction);

        Task Show();
    }

    public interface IPickerMenu
    {
        ICollection<string> Options { get; }

        int SelectedOption { get; set; }

        Task<int> Show();
    }
}

