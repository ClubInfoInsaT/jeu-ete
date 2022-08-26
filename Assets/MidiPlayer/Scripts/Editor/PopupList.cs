
using UnityEngine;
using UnityEditor;

using System;

using System.Collections;
using System.Collections.Generic;

namespace MidiPlayerTK
{
    public class PopupList : PopupWindowContent
    {
        static public CustomStyle MyStyle;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; SelectedLabel = Data != null && Data.Count >= selectedIndex + 1 ? Data[selectedIndex] : ""; }
        }

        private int selectedIndex;

        public string SelectedLabel;
        public Action<int> OnSelect;
        public bool KeepOpen;

        private Vector2 scroller;
        private List<string> Data;
        private GUIContent Content;
        private bool Selectable;

        private int winWidth = 300;
        private int winHeight = 175;
        private GUIStyle CellStyle;
        private GUIStyle TitleStyle;
        public override Vector2 GetWindowSize()
        {
            return new Vector2(winWidth, winHeight);
        }

        public PopupList(string title, bool pselectable, List<string> data)
        {
            Content = new GUIContent(title);
            Selectable = pselectable;
            if (MyStyle == null) MyStyle = new CustomStyle();
            Data = data;
            KeepOpen = false;
            CellStyle = MidiCommonEditor.styleLabelLeft;
            //TitleStyle = MidiCommonEditor.styleLabelLeft;
            //winHeight =(int)( Data.Count * CellStyle.CalcHeight(Content,300f)+ TitleStyle.CalcHeight(Content, 300f));
            //winHeight = (int)((Data.Count + 2) * CellStyle.lineHeight + TitleStyle.lineHeight);
            winHeight = (int)((Data.Count + 2) * CellStyle.lineHeight);
        }

        public override void OnGUI(Rect rect)
        {
            try
            {
                MidiCommonEditor.LoadSkinAndStyle(false);
                //GUILayout.BeginHorizontal();
                //GUILayout.Label(Content, TitleStyle);
                //if (GUILayout.Button("Close", GUILayout.Width(50), GUILayout.Height(20)))
                //    editorWindow.Close();
                //GUILayout.EndHorizontal();

                scroller = GUILayout.BeginScrollView(scroller, false, false);
                for (int index = 0; index < Data.Count; index++)
                {
                    if (Selectable)
                    {
                        GUIStyle style = (selectedIndex == index) ? MidiCommonEditor.styleListRowSelected : MidiCommonEditor.styleLabelCenter;
                        //MyStyle.BtStandard;
                        //if (SelectedIndex == index) style = MyStyle.BtSelected;
                        if (GUILayout.Button(Data[index], style))
                        {
                            selectedIndex = index;
                            SelectedLabel = Data[index];
                            if (OnSelect != null)
                                OnSelect(index);
                            if (!KeepOpen)
                                editorWindow.Close();
                        }
                    }
                    else
                    {
                        GUILayout.Label(Data[index], CellStyle);
                    }
                }
                GUILayout.EndScrollView();
            }
            catch (System.Exception ex)
            {
                MidiPlayerGlobal.ErrorDetail(ex);
            }
        }
    }
}