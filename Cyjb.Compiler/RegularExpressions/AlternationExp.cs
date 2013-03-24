﻿using System.Diagnostics;
using System.Text;

namespace Cyjb.Compiler.RegularExpressions
{
	/// <summary>
	/// 表示并联的正则表达式。
	/// </summary>
	public sealed class AlternationExp : Regex
	{
		/// <summary>
		/// 并联的第一个正则表达式。
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly Regex left;
		/// <summary>
		/// 并联的第二个正则表达式。
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly Regex right;
		/// <summary>
		/// 当前正则表达式的长度。
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int length = -2;
		/// <summary>
		/// 使用要并联的正则表达式初始化 <see cref="AlternationExp"/> 类的新实例。
		/// </summary>
		/// <param name="left">要并联的第一个正则表达式。</param>
		/// <param name="right">要并联的第二个正则表达式。</param>
		internal AlternationExp(Regex left, Regex right)
		{
			ExceptionHelper.CheckArgumentNull(left, "left");
			ExceptionHelper.CheckArgumentNull(right, "right");
			CheckRegex(left);
			CheckRegex(right);
			this.left = left;
			this.right = right;
		}
		/// <summary>
		/// 获取并联的第一个正则表达式。
		/// </summary>
		public Regex Left
		{
			get { return left; }
		}
		/// <summary>
		/// 获取并联的第二个正则表达式。
		/// </summary>
		public Regex Right
		{
			get { return right; }
		}
		///// <summary>
		///// 根据当前的正则表达式构造 NFA。
		///// </summary>
		///// <param name="nfa">要构造的 NFA。</param>
		//internal override void BuildNfa(Nfa nfa)
		//{
		//	NfaState head = nfa.CreateState();
		//	NfaState tail = nfa.CreateState();
		//	left.BuildNfa(nfa);
		//	head.Add(nfa.HeadState);
		//	nfa.TailState.Add(tail);
		//	right.BuildNfa(nfa);
		//	head.Add(nfa.HeadState);
		//	nfa.TailState.Add(tail);
		//	nfa.HeadState = head;
		//	nfa.TailState = tail;
		//}
		/// <summary>
		/// 获取当前正则表达式匹配的字符长度。变长度则为 <c>-1</c>。
		/// </summary>
		public override int Length
		{
			get
			{
				if (length == -2)
				{
					// -2 表示未初始化。
					if (left.Length == right.Length)
					{
						length = left.Length;
					}
					else
					{
						length = -1;
					}
				}
				return length;
			}
		}
		/// <summary>
		/// 返回当前对象的字符串表示形式。
		/// </summary>
		/// <returns>当前对象的字符串表示形式。</returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (left is ConcatenationExp)
			{
				builder.Append("(");
				builder.Append(left.ToString());
				builder.Append(")");
			}
			else
			{
				builder.Append(left.ToString());
			}
			builder.Append('|');
			if (right is ConcatenationExp)
			{
				builder.Append("(");
				builder.Append(right.ToString());
				builder.Append(")");
			}
			else
			{
				builder.Append(right.ToString());
			}
			return builder.ToString();
		}
	}
}
