using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BridgeManager.Source.Services {
    class ConsoleOutputter : TextWriter {
            TextBox textBox;

            public ConsoleOutputter(TextBox output) {
                textBox = output;
            }
   
            public override void Write(char value) {
                base.Write(value);
                textBox.Dispatcher.BeginInvoke(new Action(() =>
                {
                    textBox.AppendText(value.ToString());
                }));
                textBox.ScrollToEnd();
            }

            public override Encoding Encoding {
                get { return System.Text.Encoding.UTF8; }
            }
        }
    }

