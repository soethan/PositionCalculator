using NUnit.Framework;
using Moq;
using System;

namespace PositionCalculator.Tests
{
    //https://stackoverflow.com/questions/15014461/should-i-use-nunits-assert-throws-method-or-expected-exception-attribute
    public class MailerTestFixtures
    {
        Mock<IMailClient> mockMailClient;
        IMailer mailer;

        [SetUp]
        public void Init()
        {
            //Create a mock object of a MailClient class which implements IMailClient
            mockMailClient = new Mock<IMailClient>();

            //Mock the properties of MailClient
            mockMailClient
                .SetupProperty(client => client.Server, "noreply@website-admin.com")
                .SetupProperty(client => client.Port, "443");
                        
            mailer = new DefaultMailer()
            {
                From = "from@mail.com",
                To = "to@mail.com",
                Subject = "Using Moq",
                Body = "Moq is awesome"
            };
        }

        [TestCase]
        public void SendMailTestFixtureSucceed()
        {
            //Configure dummy method so that it returns true when it gets any string as parameters to the method
            mockMailClient
                .Setup(
                    client => client.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())
                )
                .Returns(true);

            //Use the mock object of MailClient instead of actual object
            var result = mailer.SendMail(mockMailClient.Object);

            //Verify that it return true
            Assert.IsTrue(result);

            //Verify that the MailClient's SendMail methods gets called exactly once when string is passed as parameters
            mockMailClient.Verify(
                client => client.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), 
                Times.Once
            );
        }

        [TestCase]
        public void SendMailTestFixtureFail()
        {
            //Configure dummy method so that it returns true when it gets any string as parameters to the method
            mockMailClient
                .Setup(
                    client => client.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())
                )
                .Throws<System.Net.Mail.SmtpException>();

            Assert.Throws<System.Net.Mail.SmtpException>(() => mailer.SendMail(mockMailClient.Object));

            //Verify that the MailClient's SendMail methods gets called exactly twice when string is passed as parameters
            mockMailClient.Verify(
                client => client.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Exactly(2)
            );
        }
    }

    public interface IMailer
    {
        string From { get; set; }
        string To { get; set; }
        string Subject { get; set; }
        string Body { get; set; }

        bool SendMail(IMailClient mailClient);
    }

    public class DefaultMailer : IMailer
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public bool SendMail(IMailClient mailClient)
        {
            try
            {
                return mailClient.SendMail(this.From, this.To, this.Subject, this.Body);
            }
            catch (Exception ex)
            {
                return mailClient.SendMail(this.From, this.To, this.Subject, this.Body);
            }
        }
    }

    public interface IMailClient
    {
        string Server { get; set; }
        string Port { get; set; }
        bool SendMail(string from, string to, string subject, string body);
    }

    public class MailClient : IMailClient
    {
        public string Server { get; set; }
        public string Port { get; set; }

        public bool SendMail(string from, string to, string subject, string body)
        {
            Console.WriteLine(string.Format("From{0};To:{1};Subject:{2};Body:{3};", from, to, subject, body));
            return true;
        }
    }
}
