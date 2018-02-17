using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AngleSharp.Parser.Html;
using Caliburn.Micro;

namespace facebook_comment_utils
{
    internal class ShellViewModel : PropertyChangedBase
    {
        private List<Person> _personsAll;

        private List<Person> _personsDistinct = new List<Person>();
        private string _personsDistinctTxt;

        private int _personsDrawCount = 1;
        private List<Person> _personsDrawn;
        private string _personsDrawnTxt;

        public int PersonsDrawCount
        {
            get => _personsDrawCount;
            set
            {
                _personsDrawCount = value;
                NotifyOfPropertyChange(() => PersonsDrawCount);
                NotifyOfPropertyChange(() => CanDrawSomePersons);
            }
        }

        public bool CanDrawSomePersons => _personsDistinct.Count > 0 && PersonsDrawCount > 0;

        public string PersonsDistinctTxt
        {
            get => _personsDistinctTxt;
            set
            {
                _personsDistinctTxt = value;
                NotifyOfPropertyChange(() => PersonsDistinctTxt);
                NotifyOfPropertyChange(() => CanDrawSomePersons);
            }
        }

        public string PersonsDrawnTxt
        {
            get => _personsDrawnTxt;
            set
            {
                _personsDrawnTxt = value;
                NotifyOfPropertyChange(() => PersonsDrawnTxt);
            }
        }

        public void GetFromClipboard()
        {
            // get clipboard context
            var clipboardText = Clipboard.GetText();

            // convert clipboard text to DOM document
            var parser = new HtmlParser();
            var document = parser.Parse(clipboardText);

            // use selectors to filter the list
            var foundPeopleDivs = document.QuerySelectorAll(".UFICommentActorName");

            if (foundPeopleDivs.Length == 0)
            {
                // display alert
                MessageBox.Show(
                    "Nie znaleziono żadnych osób. Spróbuj skopiować inny element HTML, np:\n<div class=\"UFIList\">, lub\n<div class=\"(...)UFIContainer(...)\">\n\nNierozwinięte odpowiedzi do komentarzy nie zostaną branę pod uwagę. Więcej info na: http://malbrandt.y0.pl/prepersi/losuj/", "Wyniki przeszukiwania");
                return;
            }


            // convert divs to person list
            _personsAll = new List<Person>();
            foreach (var personDiv in foundPeopleDivs)
            {
                // TODO: reduce possible errors below
                var forname = personDiv.InnerHtml.Split(' ')[0];
                var surname = personDiv.InnerHtml.Split(' ')[1];
                _personsAll.Add(new Person(forname, surname));
            }

            // remove duplicates from list
            _personsDistinct = new HashSet<Person>(_personsAll).ToList();

            // convert person distinct list to text
            var stringBuilder = new StringBuilder();

            foreach (var person in _personsDistinct)
                stringBuilder.Append($"{person}\n");

            PersonsDistinctTxt = stringBuilder.ToString();
            
            MessageBox.Show($"Ilość osób:\n - wszystkich: {foundPeopleDivs.Length}\n - unikatowych: {_personsDistinct.Count}");
        }

        public void DrawSomePersons()
        {
            // get random people
            _personsDrawn = _personsDistinct.GetRandomElements(PersonsDrawCount);

            // convert them to string
            var stringBuilder = new StringBuilder();
            foreach (var drawnPerson in _personsDrawn)
                stringBuilder.Append($"{drawnPerson}\n");
            PersonsDrawnTxt = stringBuilder.ToString();
        }
    }
}
