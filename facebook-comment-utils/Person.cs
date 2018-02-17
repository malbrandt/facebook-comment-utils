using System;

namespace facebook_comment_utils
{
    internal struct Person
    {
        string Forname;
        string Surname;

        public Person(string forname, string surname)
        {
            Forname = forname ?? throw new ArgumentNullException(nameof(forname));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
        }

        public override string ToString()
        {
            return String.Format($"{Forname} {Surname}");
        }
    }
}