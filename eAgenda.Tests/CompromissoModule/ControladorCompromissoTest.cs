using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using eAgenda.Controladores.CompromissoModule;
using eAgenda.Controladores.ContatoModule;
using eAgenda.Dominio.ContatoModule;
using eAgenda.Dominio.CompromissoModule;
using eAgenda.Controladores.Shared;

namespace eAgenda.Tests.CompromissoModule
{
    [TestClass]
    public class ControladorCompromissoTest
    {
        private ControladorCompromisso controladorCompromisso;
        private ControladorContato controladorContato;
        public ControladorCompromissoTest()
        {
            controladorContato = new ControladorContato();
            controladorCompromisso = new ControladorCompromisso();
            Db.Update("DELETE FROM [TBCOMPROMISSO]");
            Db.Update("DELETE FROM [TBCONTATO]");
        }

        [TestMethod]
        public void DeveSelecionarTodosCompromissos()
        {
            var compromisso = new Compromisso("compromissos futuros", "unidade testes", "visual studio",
                             new DateTime(2021, 03, 14), new TimeSpan(3, 00, 00), new TimeSpan(4, 00, 00), null);

            controladorCompromisso.InserirNovo(compromisso);

            var compromissosFuturosEncotrado = controladorCompromisso.SelecionarTodos();

            compromissosFuturosEncotrado.Should().HaveCount(1);
        }

        [TestMethod]
        public void DeveAtualizarCompromisso()
        {

            var contato = new Contato("José Pedro", "josepedro@gmail.com", "321654987", "JP Ltda", "Desenvolvedor");
            controladorContato.InserirNovo(contato);


            var compromisso = new Compromisso("validar campos", "unidade testes", "visual studio",
                              new DateTime(2022, 12, 13), new TimeSpan(3, 00, 00), new TimeSpan(4, 00, 00), contato);
            controladorCompromisso.InserirNovo(compromisso);


            var compromissoDois = new Compromisso("compromisso dois", "teste compromisso dois", "visual studio",
                              new DateTime(2022, 12, 13), new TimeSpan(5, 00, 00), new TimeSpan(6, 00, 00), contato);

            controladorCompromisso.Editar(compromisso.Id, compromissoDois);


            var compromissoEncotrado = controladorCompromisso.SelecionarPorId(compromisso.Id);
            compromissoEncotrado.Should().Be(compromissoDois);

        }

        [TestMethod]
        public void DeveInserirCompromisso()
        {

            var contato = new Contato("José Pedro", "josepedro@gmail.com", "321654987", "JP Ltda", "Desenvolvedor");
            controladorContato.InserirNovo(contato);


            var compromisso = new Compromisso("validar campos", "unidade testes", "visual studio",
                              new DateTime(2022, 12, 13), new TimeSpan(3, 00, 00), new TimeSpan(4, 00, 00), contato);

            controladorCompromisso.InserirNovo(compromisso);

            var compromissoEncotrado = controladorCompromisso.SelecionarPorId(compromisso.Id);

            compromissoEncotrado.Should().Be(compromisso);

        }

        [TestMethod]
        public void DeveSelecionarCompromissoFuturos()
        {
            var compromisso = new Compromisso("compromissos futuros", "unidade testes", "visual studio",
                             new DateTime(2022, 12, 13), new TimeSpan(3, 00, 00), new TimeSpan(4, 00, 00), null);

            controladorCompromisso.InserirNovo(compromisso);

            var compromissosFuturosEncotrado = controladorCompromisso.SelecionarCompromissosFuturos(new DateTime(2022, 12, 10), new DateTime(2022, 12, 14));

            compromissosFuturosEncotrado.Should().HaveCount(1);
        }

        [TestMethod]
        public void DeveInserirCompromissoSemContato()
        {
            var compromisso = new Compromisso("validar campos", "unidade testes", "visual studio",
                             new DateTime(2022, 12, 13), new TimeSpan(3, 00, 00), new TimeSpan(4, 00, 00), null);

            controladorCompromisso.InserirNovo(compromisso);

            var compromissoEncotrado = controladorCompromisso.SelecionarPorId(compromisso.Id);

            compromissoEncotrado.Should().Be(compromisso);
        }

        [TestMethod]
        public void NaoDeveInserirCompromissoMesmoDiaEHora()
        {

            var compromisso = new Compromisso("validar campos", "unidade testes", "visual studio",
                              DateTime.Today, new TimeSpan(13, 00, 00), new TimeSpan(14, 00, 00), null);

            controladorCompromisso.InserirNovo(compromisso);

            var compromissoDois = new Compromisso("validar campos", "unidade testes", "visual studio",
                              DateTime.Today, new TimeSpan(13, 00, 00), new TimeSpan(14, 00, 00), null);


            controladorCompromisso.InserirNovo(compromissoDois);

            var selecionarComproissos = controladorCompromisso.SelecionarTodos();

            selecionarComproissos.Should().HaveCount(1);
        }

    }
}
