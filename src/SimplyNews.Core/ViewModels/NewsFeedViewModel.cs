using MvvmCross.ViewModels;
using SimplyNews.Core.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using MvvmCross.Commands;
using System;

namespace SimplyNews.Core.ViewModels
{
    public class NewsFeedViewModel : MvxViewModel
    {
        private readonly INewsFeedService _newsFeedService;

        public NewsFeedViewModel(INewsFeedService newsFeedService)
        {
            _newsFeedService = newsFeedService;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            try
            {
                _newsContent = string.Empty;
                NewsCategories = new List<string>() { "All", "World", "Science", "Sports", "Technology", "Politics", "Business", "Entertainment", "Automobile", "National" };
                SelectedCategory = "World";
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private List<string> _newsCategories;
        public List<string> NewsCategories
        {
            get => _newsCategories;
            set
            {
                _newsCategories = value;
                RaisePropertyChanged(() => NewsCategories);
            }
        }

        private string _newsContent;
        public string NewsContent
        {
            get => _newsContent;
            set
            {
                _newsContent = value;
                RaisePropertyChanged(() => NewsContent);
            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged(() => SelectedCategory);
            }
        }

        private bool _isNotBusy;
        public bool IsNotBusy
        {
            get => _isNotBusy;
            set
            {
                _isNotBusy = value;
                RaisePropertyChanged(() => IsNotBusy);
            }
        }

        private MvxCommand<string> _categorySelectedCommand;
        public MvxCommand<string> CategorySelectedCommand => _categorySelectedCommand ??= new MvxCommand<string>(OnCategorySelectedAsync);

        private async void OnCategorySelectedAsync(string category)
        {
            try
            {
                if(category != null)
                    SelectedCategory = category;
                IsNotBusy = false;
                await UpdateNewsFeedAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                IsNotBusy = true;
            }
        }

        private async Task UpdateNewsFeedAsync()
        {
            NewsContent = await _newsFeedService.GetNewsFeed(SelectedCategory.ToLower());
        }
    }
}
