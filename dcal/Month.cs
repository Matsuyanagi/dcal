using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dcal
{
	internal class Month
	{
		public int Year { get; } //	～2010～
		public int MonthNo { get; } //	1～12
		public DateTime FirstDay { get; } //	
		public DayOfWeek FirstDayOfWeek => FirstDay.DayOfWeek; //	0～6
		public int LastDay { get; } //	28,29,30,31
		public bool TodayIsInThisMonth { get; } //	
		private DateTime Today { get; } //	

		//	calendar line number
		public int CalendarLineNum => ( ( int ) FirstDayOfWeek + LastDay + 6 ) / 7; //	4,5,6

		public enum DayLayout
		{
			Narrow,
			Wide,
		};

		//	コンストラクタ
		public Month( int year, int month, DateTime today )
		{
			Year = year;
			MonthNo = month;
			FirstDay = new DateTime( year, month, 1 );
			Today = today;

			//	今月判定
			TodayIsInThisMonth = false;
			if ( Year == Today.Year && MonthNo == Today.Month ) {
				TodayIsInThisMonth = true;
			}

			//	月の日数
			LastDay = DateTime.DaysInMonth( Year, MonthNo );
		}

		//	ヘッダ作成
		public IList< string > GetCalendarHeaders( DayLayout dl )
		{
			var headers = new List< string >();
			switch ( dl ) {
				case DayLayout.Wide:
					headers.Add( $" {Year,4}-{MonthNo,2}                    " );
					headers.Add( " Su  Mo  Tu  We  Th  Fr  Sa " );
					break;
				case DayLayout.Narrow:
					headers.Add( $" {Year,4}-{MonthNo,2}             " );
					headers.Add( " Su Mo Tu We Th Fr Sa" );
					break;
			}

			return headers;
		}

		//	日数行
		public IList< string > GetCalendarStrings( DayLayout dl )
		{
			var week = 0;
			var linenum = CalendarLineNum;
			var calendarString = new List< string >();

			var day = 1 - ( int )FirstDayOfWeek;

			for ( week = 0; week < linenum; week++ ) {
				var weekString = new StringBuilder();

				var lastDay = Math.Min( LastDay + 1, day + 7 );
				var weekStartDay = day;

				switch ( dl ) {
					case DayLayout.Wide:
						for ( ; day <= 0; day++ ) {
							weekString.Append( "    " );
						}

						for ( ; day < lastDay; day++ ) {
							if ( TodayIsInThisMonth && day == Today.Day ) {
								weekString.AppendFormat( ">{0,2}<", day );
							} else {
								weekString.AppendFormat( " {0,2} ", day );
							}
						}

						if ( day < weekStartDay + 7 ) {
							for ( ; day < weekStartDay + 7; day++ ) {
								weekString.Append( "    " );
							}
						}

						break;
					case DayLayout.Narrow:
						for ( ; day <= 0; day++ ) {
							weekString.Append( "   " );
						}

						for ( ; day < lastDay; day++ ) {
							if ( TodayIsInThisMonth && day == Today.Day ) {
								weekString.AppendFormat( ">{0,2}", day );
							} else {
								weekString.AppendFormat( " {0,2}", day );
							}
						}

						if ( day < weekStartDay + 7 ) {
							for ( ; day < weekStartDay + 7; day++ ) {
								weekString.Append( "   " );
							}
						}

						break;
				}

				calendarString.Add( weekString.ToString() );
			}

			return calendarString;
		}

		//	calendar string
		public IEnumerable<string> CalendarStrings()
		{
			IList< string > headers = GetCalendarHeaders( DayLayout.Wide );
			foreach ( string header in headers ) {
				yield return header;
			}

			IList< string > calendar = GetCalendarStrings( DayLayout.Wide );
			foreach ( string calendarLine in calendar ) {
				yield return calendarLine;
			}
		}
	}
}