using System;
using System.Collections.Generic;
using NDesk.Options;

namespace dcal
{
	class Program
	{
		public static bool FlagHelp { get; set; }

		static void Main( string[] args )
		{
			var cal = new Calendar();

			var p = new OptionSet
			{
				{ "h|?|help", "show help.", v => FlagHelp = true },
				{ "3", "disp 3 months.", v => cal.PrintMonth = 3 },
				{ "2", "disp 2 months.", v => cal.PrintMonth = 2 },
				{ "1", "disp 1 month.", v => cal.PrintMonth = 1 },
			};
			List< string > parameters = p.Parse( args );

			if ( FlagHelp ) {
				p.WriteOptionDescriptions( Console.Out );
			} else {

				//	パラメータとして、 月、年 を受け取る
				//	表示する月をパラメータから確定する
				//	指定がなければ今月
				var nowDate = DateTime.Now;
				var paramMonth = nowDate.Month;
				var paramYear = nowDate.Year;
				var paramDate = nowDate;

				if ( parameters.Count > 0 ) {
					var month = int.Parse( parameters[ 0 ] );
					if ( month >= 1 && month <= 12 ) {
						paramMonth = month;
					} else {
						paramMonth = nowDate.Month;
						if ( month > 1900 ) {
							paramYear = month;
						}
					}

					if ( parameters.Count > 1 ) {
						var year = int.Parse( parameters[ 1 ] );
						if ( year <= 1900 ) {
							if ( year >= 1 && year <= 12 ) {
								paramMonth = year;
							} else {
								paramYear = nowDate.Year;
							}
						} else {
							paramYear = year;
						}
					}

					paramDate = new DateTime( paramYear, paramMonth, 1 );
				}

				cal.Print( paramDate );

#if DEBUG
				Console.ReadKey();
#endif
			}
		}
	}
}
