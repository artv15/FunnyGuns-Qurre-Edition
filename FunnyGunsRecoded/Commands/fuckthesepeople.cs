using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;

namespace FunnyGunsRecoded.Commands
{
    //[CommandHandler(typeof(GameConsoleCommandHandler))]
    // Gonna remove it temporairly, because reasons
    public class fuckthesepeople : ICommand
    {
        public string Command => "fg_govnoivent";

        public string[] Aliases => null;

        public string Description => "Специально для дованов, называющих Funny Guns говноивентом без объяснения.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            /*
             I literally got pissed off by people like these. Don't ask why did I do it. I did it just because I wanted to.
             */
            response = "\n<color=green>[ Funny Guns => Govno Ivent] </color>\n" +
                "<color=red>--[ Вступление ]--</color>\n" +
                "<color=yellow>Для начала, если вы решили выполнить данную команду только потому, что она показывается в .help, то делать вам тут нечего. Если же причина другая (текст при подготовке ивента), то тут уж стоит вам толи пособолезновать</color>" +
                "<color=yellow>, толи поблагодорить за трату своего драгоценного времени.</color>\n" +
                "<color=red>--[ Вложенный труд ивентера ]--</color>\n" +
                "<color=yellow>Любой ивентер сначала придумывает свой ивент, а потом его реализует. Это не такая и простая задача, как кажется. Иногда придумывают плохие ивенты, иногда годные. Какой бы ивент ивентер не придумал, он</color>" +
                "<color=yellow>будет ценить проделанный им труд. И как только ивент-мастер слышит `фу, говноивент`, ему становится обидно.</color>\n" +
                "<color=red>--[ `Но ивент же отборное говно?` ]--</color>\n" +
                "<color=yellow>Да, ивенты бывают говном, но если уж критикуете, говорите что не так. В разработке любой программы, игры, даже этого автоивента требуется фидбек. Без него крайне сложно. Если уж</color>" +
                "<color=yellow>и хотите назвать ивент говном высшей пробы, говорите почему!</color>\n" +
                "<color=red>--[ Почему лично мне это неприятно? ]--</color>\n" +
                "<color=yellow>Я потратил только на программирование самого ивента 3 дня. Процесс идеи, наработок и тп занял больше недели-двух при написании первой версии этого автоивента. Она всё ещё есть на гитхабе, ссылки я оставлю снизу</color>" +
                "<color=yellow>(Они кликабельны).</color>\n" +
                "<color=red>--[ Ссылки к параграфу выше ]--</color>\n" +
                "<color=yellow><link=\"https://github.com/artv15/FunnyGuns\">Первая версия автоивента.</link></color>\n" +
                "<color=yellow><link=\"https://github.com/artv15/FunnyGuns-Qurre-Edition\">Текущая версия автоивента</link></color>\n" +
                "<color=red>--[ В заключение ]--</color>\n" +
                "<color=yellow>Всегда есть ивенты, которые вам кажутся скучными (aka говном). Ради бога, говорите ивентерам, что у них в ивенте не так, чтобы они не стояли на одном месте и исправляли свои ошибки!</color>";
            return true;
        }
    }
}
