using BasicUI.Controls;
using BasicUI.Native;
using Humanizer;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BasicUI.FileBrowser
{
    public static class BrowserWindow
    {
        public static void CreateBindings(Binder<BrowserViewModel> binder, Window w)
        {
            binder.BindPropChangedTwoWay
            (
                w.FindControlWithId<TextBox>("pathBox"),
                (box, vm) => box.Text = vm.CurrentDirectory,
                (vm, box) =>
                {
                    try
                    {
                        vm.CurrentDirectory = box.Text;
                    }
                    catch (Exception ex)
                    {
                        w.FindControlWithId<ExceptionBox>("Error").Show(ex);
                    }
                }
            );

            binder.BindSpecificPropChanged
            (
                vm => vm.Files,
                w.FindControlWithId<WrappingContainer>("currentFiles"),
                (vm, container) => FillContainer(w.FindControlWithId<ExceptionBox>("Error"), container)
            );

            binder.BindSpecificPropChanged
            (
                vm => vm.SelectedPath,
                w.FindControlWithId<MessageBox>("Are you sure?"),
                (vm, box) => box.Message = $"Open \"{Path.GetFileName(vm.SelectedPath)}\"?"
            );

            binder.BindPropChangedTwoWay
            (
                w.FindControlWithId<MessageBox>("Are you sure?"),
                (box, vm) => box.IsOpen = vm.OpenFilePromptShown,
                (vm, box) => vm.OpenFilePromptShown = box.IsOpen
            );

            binder.BindSpecificPropChanged
            (
                vm => vm.Sorting,
                w.FindControlWithId<MenuItem>("Name"),
                (vm, item) => item.Selected = vm.Sorting == Sorting.Name
            );

            binder.BindSpecificPropChanged
            (
                vm => vm.Sorting,
                w.FindControlWithId<MenuItem>("Size"),
                (vm, item) => item.Selected = vm.Sorting == Sorting.Size
            );

            binder.BindSpecificPropChanged
            (
                vm => vm.Sorting,
                w.FindControlWithId<MenuItem>("Date Modified"),
                (vm, item) => item.Selected = vm.Sorting == Sorting.Modified
            );

            binder.BindSpecificPropChanged
            (
                vm => vm.AnimateLabels,
                w.FindControlWithId<MenuItem>("Animate Labels"),
                (vm, item) => item.Selected = vm.AnimateLabels
            );
        }

        static void FillContainer(ExceptionBox errBox, WrappingContainer container)
        {
            var viewModel = container.GetBinding<BrowserViewModel>();

            container.Clear();

            foreach (string path in viewModel.Files)
            {
                string ext = Path.GetExtension(path).Replace(".", "");

                if (Directory.Exists(path))
                {
                    ext = "folder";
                }

                if (!ImageHelper.LoadedImages.Any(d=>d == ext))
                {
                    ext = "file";
                }

                var p = new Panel(path)
                {
                    Size = new Vector2(96, 96),
                    WindowFlags = WindowFlags.NoInputs,
                    Children =
                    {
                        new ImageBox(ext) { Size = new Vector2(64, 64) },
                        new Text
                        {
                            Content = Path.GetFileName(path),
                            Color = Color.Black
                        }
                    }
                };

                List<string> tooltipLines = new List<string>
                {
                    Path.GetFileName(path)
                };

                if (File.Exists(path))
                {
                    var info = new FileInfo(path);

                    tooltipLines.Add($"Size: {info.Length.Bytes().ToString("#.##")}");
                    tooltipLines.Add($"Modified: {info.LastWriteTime.ToString("MM/dd/yy h:mm tt")}");
                }

                string toolTip = String.Join(Environment.NewLine, tooltipLines);

                p.PostRender = panel =>
                {
                    //Avoiding foreach closure issues
                    string curPath = path;
                    //Get a reference to the label within this panel
                    var text = (panel as Container).ElementAt(1) as Text;

                    if (ImGui.IsLastItemHovered())
                    {
                        ImGui.SetTooltip(toolTip);

                        if (panel.GetBinding<BrowserViewModel>().AnimateLabels && text.Content.Length > 12)
                        {
                            //Start the offset a bit to the right, to make the animation smoother
                            if (text.Position == Vector2.Zero)
                            {
                                text.Position = new Vector2(8, text.Position.Y);
                            }

                            //Keep moving the text left until we hover out
                            text.Position = new Vector2(text.Position.X - 0.4f, text.Position.Y);
                        }
                    }
                    else
                    {
                        //Set the text back to the default position when we hover out
                        text.Position = Vector2.Zero;
                    }

                    //If our item was clicked, and it was a double click, display the file open dialog
                    //or open the folder
                    if (ImGuiNative.igIsItemClicked(0) && ImGui.IsMouseDoubleClicked(0))
                    {
                        viewModel.SelectedPath = curPath;

                        if (Directory.Exists(curPath))
                        {
                            try
                            {
                                viewModel.NavigateToSelected();
                            }
                            catch (Exception ex)
                            {
                                errBox.Show(ex);
                            }
                        }
                        else
                        {
                            viewModel.OpenFilePromptShown = true;
                        }
                    }
                };

                container.Add(p);
            }
        }

        public static void Setup(Window w)
        {
            //Set up the file open dialog
            w.RootContainer.Add(new ExceptionBox("Error"));

            w.RootContainer.Add(new MessageBox("Are you sure?", "Are you sure you want to open this file?", "Yeah", "Nope")
            {
                OptionSelected = (sender, opt) =>
                {
                    //If the user says yes, open the file with explorer
                    if (opt == "Yeah")
                    {
                        sender.GetBinding<BrowserViewModel>().OpenSelected();
                    }
                }
            });

            //Set up the main frame
            w.RootContainer.Add(new Frame
            {
                Size = new Vector2(1280, 698),
                Position = new Vector2(0, 22),
                WindowFlags = WindowFlags.NoResize | WindowFlags.NoTitleBar | WindowFlags.NoMove,
                Children =
                {
                    new HorizontalLayout
                    {
                        new Button("goUp")
                        {
                            Text = " ^ ",
                            OnClick = btn =>
                            {
                                btn.GetBinding<BrowserViewModel>().NavigateUpDirectory();
                            }
                        },
                        new TextBox("pathBox")
                        {
                            Width = 1235,
                            InputTextFlags = InputTextFlags.EnterReturnsTrue
                        }
                    },
                    new Panel
                    {
                        WindowFlags = WindowFlags.AlwaysAutoResize,
                        Children =
                        {
                            new WrappingContainer("currentFiles")
                            {
                                EstimatedItemSize = 96
                            }
                        }
                    }
                }
            });
        }
    }
}
