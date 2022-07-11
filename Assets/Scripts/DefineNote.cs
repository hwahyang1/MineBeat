using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] Minebeat
 * Desciption
 */
namespace MineBeat
{
	 /*
	 * [Enum] NoteType
	 * 노트의 종류를 정의합니다.
	 */
	[System.Serializable]
	public enum NoteType
	{
		Normal,
		Vertical,
		SizeChange,
		BoxColorChange,
		BlankS,
		BlankE,
		ImpactLine,
		PreviewS,
		PreviewE
	}

	/*
	 * [Enum] NoteDirection
	 * 박스를 기준으로 노트에 어느 방향에 존재하는지 표현합니다.
	 */
	[System.Serializable]
	public enum NoteDirection
	{
		Up,
		Down,
		Left,
		Right,
		None
	}

	/*
	 * [Enum] NoteColor
	 * (Normal/Vertical 한정) 노트의 색상을 정의합니다.
	 */
	[System.Serializable]
	public enum NoteColor
	{
		White,
		Skyblue,
		Blue,
		Green,
		Orange,
		Purple
	}

	/*
	 * [Class] NotePosition
	 * 노트의 위치를 정의합니다.
	 */
	[System.Serializable]
	public class NotePosition
	{
		public ushort x;
		public ushort y;

		public NotePosition()
		{
			x = 0;
			y = 0;
		}
		public NotePosition(ushort x, ushort y)
		{
			this.x = x;
			this.y = y;
		}
		public NotePosition(int x, int y)
		{
			this.x = (ushort)x;
			this.y = (ushort)y;
		}
	}

	/*
	 * [Class] Note
	 * 노트의 정보를 정의합니다.
	 */
	[System.Serializable]
	public class Note
	{
		public float timeCode;
		public NoteType type;
		public NoteColor color;
		public NotePosition position;
		public NoteDirection direction;

		public Note(float timeCode, NoteType type)
		{
			this.timeCode = timeCode;
			this.type = type;

			color = NoteColor.White;
			position = new NotePosition(0, 0);
			direction = NoteDirection.None;
		}
		public Note(float timeCode, NoteType type, NotePosition position)
		{
			this.timeCode = timeCode;
			this.type = type;
			this.position = position;

			color = NoteColor.White;
			direction = NoteDirection.None;
		}
		public Note(float timeCode, NoteType type, NoteColor color, NotePosition position)
		{
			this.timeCode = timeCode;
			this.type = type;
			this.color = color;
			this.position = position;

			direction = NoteDirection.None;
		}
		public Note(float timeCode, NoteType type, NoteColor color, NotePosition position, NoteDirection direction)
		{
			this.timeCode = timeCode;
			this.type = type;
			this.color = color;
			this.position = position;
			this.direction = direction;
		}
	}
}
