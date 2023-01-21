using System.Linq;

namespace CBLoLManager.Configuration;

using Model;

public class MenuMessageInitializer : Initializer
{
    public override void Initialize()
    {
        var messages = MenuMessages.All;

        if (messages.Count() > 0)
            return;
        
        #region Comuns

        add("Tão bugado quanto o client do LoL", 16);
        add("Aqui você não fica 0/11, só seus jogadores podem ficar", 16);
        add("Nos faça um Pix Diff para prolongar o projeto", 16);
        add("Segue ai o @trevisharp", 16);
        add("Se achar algum bug... Vamos lançar uma skin para lux", 16);
        add("Tradicional", 16);
        add("Intrépido", 16);
        add("Furioso", 16);
        add("Aqui você pode ser o Corradini", 16);
        add("Aqui você pode ser o Paada", 16);
        add("MITOLÓGICO", 16);
        add("PEEEEEEIIIII NA TORRE", 16);
        add("Cuide bem do seu Blue", 16);
        add("Cuide bem do seu Red", 16);
        add("Beba agua", 16);
        add("Cuidado, LoL vicia!", 16);

        #endregion

        #region Incomuns

        add("Só agradece", 8);
        add("IRIRIRIRIRIRIRI", 8);
        add("É o Robs", 8);
        add("Somos Rangernation", 8);
        add("No rio você não perde!", 8);
        add("Coreano não é Deus", 8);
        add("Oi sumido.", 8);
        add("A odd para você ganhar o CBLoL é mais de 8000", 8);

        #endregion

        #region Raras

        add("A sogra desce", 4);
        add("Nunca jogue um pão nos seus colegas de time", 4);
        add("E mais um caso de passáro registrado no CBLoL", 4);
        add("Calibra esse smite ai", 4);

        #endregion

        #region Épicas

        add("Rexpeita o Rato", 2);
        add("O lobo vai comer seu...", 2);

        #endregion

        #region Lendárias

        add("Eu era o cupado velho? Me kicka agora. -Esinha", 1);

        #endregion

        void add(string message, int weigth)
        {
            messages.Add(new MenuMessage
            {
                Message = message,
                Weigth = weigth
            });
        }
    }
}