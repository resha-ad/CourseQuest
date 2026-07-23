using System.Collections.Generic;
using UnityEngine;

public class MyQuestions : MonoBehaviour
{
    public class Question
    {
        public string questionText;
        public List<string> options;
        public int correctAnswerIndex; // Index of the correct answer in the options list
        public List<string> feedback; // Feedback for each wrong answer

        public Question(string questionText, List<string> options, int correctAnswerIndex, List<string> feedback)
        {
            this.questionText = questionText;
            this.options = options;
            this.correctAnswerIndex = correctAnswerIndex;
            this.feedback = feedback;
        }
    }

    // Questions grouped by modules
    public List<Question> allQuestions = new List<Question>
    {
        // Module 1: Awareness and Basic Practices
        new Question(
            "1. What should Priya do first after receiving her company-issued laptop?",
            new List<string>
            {
                "A) Change the default password to a strong, unique one.",
                "B) Install personal apps for convenience.",
                "C) Share her credentials with IT colleagues.",
                "D) Use the default password provided."
            },
            0,
            new List<string>
            {
                "Correct! It's important to change the default password to ensure security.",
                "Installing personal apps could introduce malware or unauthorized software, compromising security.",
                "Sharing credentials violates company policy and exposes sensitive systems to misuse.",
                "Using the default password makes her account vulnerable to brute-force attacks."
            }
        ),
        new Question(
            "2. Priya receives an email from 'HR' asking her to confirm her personal details by clicking a link. What should she do?",
            new List<string>
            {
                "A) Click the link and provide the requested details.",
                "B) Report the email to IT security.",
                "C) Ignore the email completely.",
                "D) Forward the email to her friends for advice."
            },
            1,
            new List<string>
            {
                "Clicking the link could lead to phishing, compromising her personal and company information.",
                "Correct! Reporting suspicious emails to IT helps prevent phishing attacks.",
                "Ignoring the email might allow the threat to persist and target others.",
                "Forwarding the email spreads the risk and delays the proper action."
            }
        ),
        
        // Module 2: Device Security and Safe Practices
        new Question(
            "3. During a coffee break, Priya leaves her laptop unattended at her desk. What should she have done to protect it?",
            new List<string>
            {
                "A) Keep the screen turned on for convenience.",
                "B) Log out or lock the device.",
                "C) Ask a colleague to watch over it.",
                "D) Hide it under papers on her desk."
            },
            1,
            new List<string>
            {
                "Leaving the screen on allows unauthorized access to sensitive files.",
                "Correct! Logging out or locking the device ensures no unauthorized access.",
                "A colleague might not have the same awareness of security protocols.",
                "Hiding the laptop doesn’t secure the data or prevent access."
            }
        ),
        new Question(
            "4. Priya plugs her phone into her office computer to charge. What potential risk does this pose?",
            new List<string>
            {
                "A) None, it’s just charging.",
                "B) It could lead to data theft or malware transfer.",
                "C) It slows down her computer.",
                "D) It voids the warranty of the office device."
            },
            1,
            new List<string>
            {
                "Overlooking this risk could allow malware to infiltrate the network via the USB connection.",
                "Correct! Connecting devices can transfer malware or unauthorized files.",
                "It underestimates the actual risks of data breaches.",
                "It results in data breaches."
            }
        ),
        
        // Module 3: Social Engineering Threats
        new Question(
            "5. A stranger in the office claims to be an IT technician and asks for Priya’s password to fix a 'system issue.' What should she do?",
            new List<string>
            {
                "A) Provide the password to help them.",
                "B) Ignore them completely.",
                "C) Request their ID and then share the password.",
                "C) Politely refuse and report to IT.",
            },
            3,
            new List<string>
            {
                "Sharing her password gives unauthorized access to sensitive systems.",
                "Even with an ID, sharing credentials violates security protocols.",
                "Ignoring the situation might allow a breach to continue undetected.",
                "Correct! Politely refusing and reporting ensures security."
            }
        ),
        
        // Module 4: Handling Sensitive Information
        new Question(
            "6. Priya notices a coworker’s printed document with sensitive data left on a shared printer. What should she do?",
            new List<string>
            {
                "A) Leave it there; it’s not her responsibility.",
                "B) Return it to the coworker directly.",
                "C) Take it and dispose of it securely.",
                "D) Read it to see if it’s important."
            },
            2,
            new List<string>
            {
                "Ignoring sensitive documents increases the risk of data leaks.",
                "Returning it directly might expose her to accusations of mishandling.",
                "Correct! Disposing of sensitive documents securely prevents data leaks.",
                "Reading the document breaches privacy and violates company policies."
            }
        ),
        new Question(
            "7. While working remotely, Priya uses a public Wi-Fi network. What is the best way to secure her connection?",
            new List<string>
            {
                "B) Use a VPN to encrypt her connection.",
                "B) Rely on the public network’s password for security.",
                "C) Avoid accessing sensitive information.",
                "D) Connect without any additional security measures."
            },
            0,
            new List<string>
            {
                "Correct! A VPN provides encryption to protect data over public networks.",
                "A public network password does not ensure a secure connection.",
                "Even avoiding sensitive sites does not prevent interception of her online activities.",
                "Unsecured connections can expose all her data to attackers."
            }
        ),
        new Question(
            "8. Priya needs to dispose of an old USB drive with company files. What should she do?",
            new List<string>
            {
                "A) Delete the files and throw it in the trash.",
                "B) Use secure wiping software or physically destroy the drive.",
                "C) Give it to a coworker to reuse.",
                "D) Format the drive and donate it."
            },
            1,
            new List<string>
            {
                "Deleting files does not remove them permanently, risking data recovery.",
                "Correct! Secure wiping or physical destruction ensures complete data erasure.",
                "Reusing the drive without proper cleaning risks data leaks.",
                "Formatting does not ensure the complete erasure of sensitive data."
            }
        ),
        new Question(
            "9. Priya receives a file-sharing link from a coworker through a messaging app. What should she verify before opening it?",
            new List<string>
            {
                "A) That the link opens without issues.",
                "B) The speed of her internet connection.",
                "C) The sender’s identity and the file’s relevance.",
                "D) Whether the file looks interesting."
            },
            2,
            new List<string>
            {
                "Opening unknown links without verification risks malware infections.",
                "It ignores the importance of verifying file authenticity and sender identity.",
                "Correct! Verifying sender and relevance helps ensure the file’s safety.",
                "It can be vulnerable."
            }
        ),
        new Question(
            "10. Priya notices her screen flickering and receiving random pop-ups while browsing. What should she do?",
            new List<string>
            {
                "A) Ignore it and continue working.",
                "B) Restart her computer.",
                "C) Report the issue to IT immediately.",
                "D) Disable the pop-ups manually."
            },
            2,
            new List<string>
            {
                "Ignoring the issue might allow malware to escalate.",
                "Restarting could delay identifying the root cause of the problem.",
                "Correct! Reporting it immediately helps IT address potential threats.",
                "Manually disabling pop-ups does not address potential underlying malware."
            }
        ),
        
        // Module 5: Advanced Threats and Incident Response
        new Question(
            "11. Priya receives a request to download software from a third-party website to complete her work. What should she do?",
            new List<string>
            {
                "A) Download it since it’s required for her task.",
                "B) Confirm with her IT department before proceeding.",
                "C) Check reviews of the software online.",
                "D) Use a personal device to download it."
            },
            1,
            new List<string>
            {
                "Downloading unverified software risks introducing malware.",
                "Correct! Confirming with IT ensures the software is safe.",
                "Online reviews might not ensure the software is safe or compatible with company systems.",
                "Using personal devices introduces additional security risks."
            }
        ),
        new Question(
            "12. A coworker urgently asks for Priya’s login details to complete a project. What should she do?",
            new List<string>
            {
                "A) Share her credentials to avoid delaying the project.",
                "B) Report the coworker to IT immediately.",
                "C) Ignore the request entirely.",
                "D) Offer to assist without sharing her details.",
            },
            3,
            new List<string>
            {
                "Sharing credentials violates security policies and risks unauthorized access.",
                "Reporting without understanding intent might harm team dynamics unnecessarily.",
                "Ignoring the request might cause unnecessary delays and misunderstandings.",
                "Correct! Offering help without sharing details maintains security."
            }
        ),
        new Question(
            "13. Priya notices a suspicious device plugged into a company computer in a shared workspace. What should she do?",
            new List<string>
            {
                "A) Unplug the device and discard it.",
                "B) Inform IT security immediately.",
                "C) Ignore it and assume it’s for authorized use.",
                "D) Attempt to identify the device’s owner."
            },
            1,
            new List<string>
            {
                "Unplugging without proper knowledge might interfere with investigations or operations.",
                "Correct! Informing IT ensures proper handling of the situation.",
                "Ignoring a suspicious device could enable data theft or network breaches.",
                "Attempting to identify the owner delays the escalation of a potential threat."
            }
        ),
        new Question(
            "14. Priya’s friend sends her a funny video link through her work email. What should she consider before opening it?",
            new List<string>
            {
                "A) Open it since it’s from someone she trusts.",
                "B) Verify the link with her friend through a different communication method.",
                "C) Scan the link using antivirus software.",
                "D) Ignore the message entirely."
            },
            1,
            new List<string>
            {
                "Even trusted contacts can unknowingly send malicious links.",
                "Correct! Verifying the link with her friend avoids security risks.",
                "While scanning is a good practice, it should be coupled with verifying the sender’s intent.",
                "Ignoring might miss a legitimate communication but doesn’t address potential risks."
            }
        ),
        new Question(
            "15. Priya notices a popup on her screen claiming that her system is infected and urging her to download antivirus software. What should she do?",
            new List<string>
            {
                "A) Click the link to download the antivirus immediately.",
                "B) Restart her computer and check if the popup disappears.",
                "C) Close the popup and report it to IT.",
                "D) Ignore the message completely."
            },
            2,
            new List<string>
            {
                "Downloading software from unverified sources might install malware.",
                "Restarting doesn’t address the root cause of the issue.",
                "Correct! Reporting it ensures proper action is taken to prevent threats.",
                "Ignoring might allow the threat to escalate or persist."
            }
        )
    };

    public List<Question> GetQuestions()
    {
        return allQuestions; // Returns all questions
    }
}
