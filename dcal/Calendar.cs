using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace dcal
{
	internal class Calendar
	{
		public int PrintMonth { get; set; }

		public Calendar()
		{
			PrintMonth = 3;
		}


		public void Print( DateTime calendarDate )
		{
			var today = DateTime.Now;

			switch( PrintMonth ){
				case 1:
					Month1( today, calendarDate );
					break;
				case 2:
					Month2( today, calendarDate );
					break;
				case 3:
					Month3( today, calendarDate );
					break;
				default:
					Debug.Assert( false );
					break;
			}
		}

		//	print 1 month.
		private void Month1( DateTime today, DateTime calendarDate )
		{
			var month = new Month( calendarDate.Year, calendarDate.Month, today );
			PrintStrings( month.CalendarStrings(), Console.Out );
		}

		//	print 2 months.
		private void Month2( DateTime today, DateTime calendarDate )
		{
			var nextYear = calendarDate.Year;
			var nextMonthNo = calendarDate.Month + 1;
			if ( nextMonthNo == 13 ){
				nextYear += 1;
				nextMonthNo = 1;
			}
			var separator = " | ";

			var curMonth = new Month( calendarDate.Year, calendarDate.Month, today );
			var nextMonth = new Month( nextYear, nextMonthNo, today );

			var curHeaders = curMonth.GetCalendarHeaders( Month.DayLayout.Wide );
			var nextHeaders = nextMonth.GetCalendarHeaders( Month.DayLayout.Wide );

			var headers = AppendStrings( curHeaders, nextHeaders, separator );
			PrintStrings( headers, Console.Out );

			var days = AppendStrings( curMonth.GetCalendarStrings( Month.DayLayout.Wide ), nextMonth.GetCalendarStrings( Month.DayLayout.Wide ), separator );
			PrintStrings( days, Console.Out );
		}

		//	print 3 months.
		private void Month3( DateTime today, DateTime calendarDate ) {
			var nextYear = calendarDate.Year;
			var nextMonthNo = calendarDate.Month + 1;
			if ( nextMonthNo == 13 ){
				nextYear += 1;
				nextMonthNo = 1;
			}
			var prevYear = calendarDate.Year;
			var prevMonthNo = calendarDate.Month - 1;
			if ( prevMonthNo == 0 ){
				prevYear -= 1;
				prevMonthNo = 12;
			}
			var separator = " |";

			var prevMonth = new Month( prevYear, prevMonthNo, today );
			var curMonth = new Month( calendarDate.Year, calendarDate.Month, today );
			var nextMonth = new Month( nextYear, nextMonthNo, today );

			var prevHeaders = prevMonth.GetCalendarHeaders( Month.DayLayout.Narrow );
			var curHeaders = curMonth.GetCalendarHeaders( Month.DayLayout.Narrow );
			var nextHeaders = nextMonth.GetCalendarHeaders( Month.DayLayout.Narrow );

			var headers = AppendStrings( curHeaders, nextHeaders, separator );
			headers = AppendStrings( prevHeaders, headers, separator );
			PrintStrings( headers, Console.Out );

			var days = AppendStrings( curMonth.GetCalendarStrings( Month.DayLayout.Narrow ), nextMonth.GetCalendarStrings( Month.DayLayout.Narrow ), separator );
			days = AppendStrings( prevMonth.GetCalendarStrings( Month.DayLayout.Narrow ), days, separator );
			PrintStrings( days, Console.Out );
		}

		//	append each line strings.
		private static IList< string > AppendStrings( IList< string > leftLines, IList< string > rightLines, string separator )
		{
			var lineNum = Math.Max( leftLines.Count, rightLines.Count );
			var s1LineLength = 0;
			var s2LineLength = 0;
			var lines = new List< string >( 10 );
			var sb = new StringBuilder( 256 );

			for ( var i = 0; i < lineNum; i++ ){
				sb.Clear();
				if ( i >= leftLines.Count ){
					sb.Append( new string( ' ', s1LineLength ) );
					sb.Append( separator );
					sb.Append( rightLines[ i ] );
					lines.Add( sb.ToString() );
					s2LineLength = rightLines[ i ].Length;
				} else if ( i >= rightLines.Count ){
					sb.Append( leftLines[ i ] );
					sb.Append( separator );
					sb.Append( new string( ' ', s2LineLength ) );
					lines.Add( sb.ToString() );
					s1LineLength = leftLines[ i ].Length;
				} else{
					sb.Append( leftLines[ i ] );
					sb.Append( separator );
					sb.Append( rightLines[ i ] );
					lines.Add( sb.ToString() );

					s1LineLength = leftLines[ i ].Length;
					s2LineLength = rightLines[ i ].Length;
				}
			}

			return lines;
		}

		//	文字列配列表示
		private static void PrintStrings( IEnumerable< string > strings, TextWriter writer )
		{
			foreach ( var s in strings ){
				writer.WriteLine( s );
			}
		}
	}
}
