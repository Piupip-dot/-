// Программа для выяснения опасен ли введённый порт или нет
using System;
class Programm
{
    // Я перевёл всё, что было на английском с помощью переводчика, чтобы так было удобнее, с помощью переводчика, потому что английский у меня хромает
    // Это параллельные массивы, они все ваимосвязанны, если одному задать условное значение 3, то все они покажут 3й шаг, точнее вариант
    static readonly int[] RiskyPorts = { 21, 23, 445, 3389, 5900 };
    static readonly string[] RiskyNames = { "FTP", "Telnet", "SMB", "RDP", "MySQL", "VNC" };
    static readonly string[] RiskyReasons =
    {
        // В зависимости от того какой из опасных портов будет, выдаёт ответ
        "отправляет пароли в виде текста",
        "отправляет все в виде текста; ботнет Mirai распространяется через него",
        "WannaCry распространился через этот порт в 2017 году",
        "удаленный рабочий стол - обычная цель для подбора пароля",
        "база данных никогда не должна быть доступна из Интернета",
        "удаленный доступ к экрану, часто со слабым паролем"
    };
    static readonly int[] RiskPoints = { 2, 3, 3, 3, 2, 2 };
    // Интерфейс для пользователя, и возможность им пользоваться
    static void Main()
    {
        Console.WriteLine("=== Анализатор портовых рисков ===");
        Console.WriteLine("Введите открытые порты через запятую. Пустая строка для выхода.");
        while (true)
        {
            Console.WriteLine();
            Console.Write("Порт:");
            string input = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(input))
            {
                break;
            }

            int[] ports = ParsePorts(input);

            if (ports.Length == 0)
            {
                Console.WriteLine("Нет портов для анализа. Пробовать снова.");
            }
            else
            {
                AnalyzePorts(ports);
                Console.WriteLine("Досвидания!");
            }

            // Эту часть я сделал с помощью ИИ, потому что я сам не смог понять, как сделать так, чтобы "parts" принимало какое-либо значение из-за чего у меея была ошибка
            // Условия для порта, какие значения и используемые символы должны использоваться
            static int[] ParsePorts(string input)
            {
                string[] parts = input.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var ports = new List<int>();
                foreach (string part in parts)
                {
                    if (!int.TryParse(part, out int port) || port < 1 || port > 65535)
                    {
                        Console.WriteLine($"Недопустимый порт \"{part}\".");
                        Console.WriteLine("Порт — это целое число от 1 до 65535.");
                        return Array.Empty<int>();
                    }

                    ports.Add(port);
                }

                return ports.ToArray();
            }

            // Анализ и вердикт о том, опасен ли код или нет
                static void AnalyzePorts(int[] ports)
            {
                // Это было сделанно, для того чтобы они принимали какое-либо значение, без этого выдавало ошибку, как в 49 - 66 строках
                int riskyCount = 0;
                int rickScore = 0;

                foreach (int port in ports)
                {
                    int index = FindRiskIndex(port);

                    if (index == -1)
                    {
                        Console.WriteLine($"[ OK ] {port}: никаких известных проблем нет");
                    }
                    else
                    {
                        string name = RiskyNames[index];
                        string reason = RiskyReasons[index];

                        Console.WriteLine($"[RISK] {port} ({name}): {reason}");
                        riskyCount++;
                        rickScore += RiskPoints[index];
                    }
                }

                int riskScore = 0;
                Console.WriteLine($"Рискованные порты: {riskyCount} of {ports.Length}");
                Console.WriteLine($"Оценка риска: {riskScore}");
                Console.WriteLine($"Вердикт: {GetVerdict(riskScore)}");
            }

            // честно, не могу сформулировать и объяснить, и кокнретного понимания тоже нет
            static int FindRiskIndex (int port)
            {
                for (int i = 0; i < RiskyPorts.Length; i++)
                {
                    if (RiskyPorts[i] == port)
                    {
                        return i;
                    }
                }
                // Если указать "return 0", то выйдет баг, при котором программа считает нулевой массив как за действие и выдаст ответ "FTP" с портом "21"
                return -1;
            }

            // В зависимости от ответа, указывает нужно ли обращать внимание на порт или нет и нужно ли его проверять и удостоверяться в нём
            static string GetVerdict(int riskScore)
            {
                if (riskScore == 0)
                {
                    return "НИКАКИХ ИЗВЕСТНЫХ РИСКОВ";
                }

                if (riskScore <= 2)
                {
                    return "НУЖДАЕТСЯ ВО ВНИМАНИИ";
                }

                return "КРИТИЧЕСКИЙ";
            }
        }
    }
}