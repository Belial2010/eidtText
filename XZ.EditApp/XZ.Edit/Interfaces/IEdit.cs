using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XZ.Edit.Entity;
using XZ.Edit.Forms;

namespace XZ.Edit.Interfaces {
    public interface IEdit {
        /// <summary>
        /// 获取当前句柄
        /// </summary>
        IntPtr GetHandle { get; }

        /// <summary>
        /// 
        /// </summary>
        Graphics GetGraphics { get; }

        Parser GetParser { get; }

        CursorAndIME GetCursor { get; }

        int GetRepealCount { get; }

        /// <summary>
        /// 获取高度
        /// </summary>
        int GetHeight { get; }

        /// <summary>
        /// 获取宽度
        /// </summary>
        int GetWidth { get; }

        /// <summary>
        /// 绘制右侧行号码背景颜色
        /// </summary>
        Color LeftNumBackGroundColor { get; set; }

        /// <summary>
        /// 绘制右侧行号码分隔符
        /// </summary>
        Color LeftNumSeparatorColor { get; set; }

        /// <summary>
        /// 绘制右侧行号码字体颜色
        /// </summary>
        Color LeftNumColor { get; set; }


        /// <summary>
        /// 选中的行背景颜色
        /// </summary>
        Color SelectLineColor { get; set; }

        /// <summary>
        /// 折叠背景颜色
        /// </summary>
        Color SelectBackGroundColor { get; set; }

        /// <summary>
        /// 折叠颜色
        /// </summary>
        Color PuckerColor { get; set; }

        /// <summary>
        /// 查找的背景颜色
        /// </summary>
        Color FindBackGroundColor { get; set; }

        /// <summary>
        /// 查找但是被选中的背景颜色
        /// </summary>
        Color FindSelectBackGroundColor { get; set; }

        /// <summary>
        /// 获取滚动条垂直高度值
        /// </summary>
        int GetVerticalScrollValue { get; }

        /// <summary>
        /// 获取滚动条水平长度值
        /// </summary>
        int GetHorizontalScrollValue { get; }

        /// <summary>
        /// 获取滚动条的宽度
        /// </summary>
        int GetScrollWidth { get; }

        /// <summary>
        /// 选择行的样式
        /// </summary>
        ESelectLineStyle SelectLineStyle { get; set; }

        /// <summary>
        /// 折叠行的背景颜色
        /// </summary>
        Color PuckeBackGrounColor { get; set; }

        /// <summary>
        /// 获取将容器控件分配给的窗体
        /// </summary>
        Form GetParentForm { get; }

        event CompletionWindowSelectEventHandler CompletionWindowSelectEvent;

        /// <summary>
        /// 添加自动补全窗口
        /// </summary>
        /// <param name="cw"></param>
        void AddCompletionWindow(CompletionWindow cw);

        /// <summary>
        /// 让系统重绘
        /// </summary>
        void Invalidate();

        /// <summary>
        /// 让当前窗体获取到焦点
        /// </summary>
        void GetFocues();

        void SetVerticalScrollValue(bool changeMinSize = true);


        bool KeyWordDownContains(Keys k);

        /// <summary>
        /// 设置水平最大宽度
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void SetMaxScollMaxWidth(int width);

        /// <summary>
        /// 改变自动滚动最小尺寸
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void ChangeScollSize(int width = 0, int height = 0);

        /// <summary>
        /// 设置水平滚动条值
        /// </summary>
        /// <param name="lorR">左移动 -1 右移  1</param>
        /// <param name="resetCursor">是否重新设置光标</param>
        void SetHorizontalScrollValue(int value, int lorR, bool resetCursor = false);

        /// <summary>
        /// 获取光标对应的屏幕坐标
        /// </summary>
        /// <returns></returns>
        Point GetPointCursorForScreen(Point point);

        Func<string, WFontColor> SetWordStyleEvent { get; set; }

        /// <summary>
        /// 设置文本修改
        /// </summary>
        void SetChangeText();
    }
}
