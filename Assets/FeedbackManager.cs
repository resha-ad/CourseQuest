using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    // Dictionary to store feedback by category and score range
    public Dictionary<string, Dictionary<string, string[]>> categoryOneFeedbacks = new Dictionary<string, Dictionary<string, string[]>>
    {
        {
            "Dietary Preferences", new Dictionary<string, string[]>
            {
                {
                    "35-40 Points (Excellent)", new string[]
                    {
                        "You have an exemplary diet. Your intake of fruits, vegetables, whole grains, and lean proteins is impressive. Keep this up by continuing to explore diverse recipes and incorporating seasonal produce for added nutrition.",
                        "Your diet is very balanced with minimal processed foods and a focus on whole ingredients. For variety, consider incorporating more plant-based meals, such as lentil soups or quinoa salads.",
                        "Your commitment to healthy eating is evident. You’re consuming a wide range of nutrients. Continue this by adding different types of beans and nuts, and experimenting with new cooking methods.",
                        "Your low intake of sugary foods and high consumption of organic produce is excellent. Maintain this by exploring new ways to enjoy fruits and vegetables, such as smoothies or homemade veggie chips.",
                        "Your meal planning and preparation are top-notch. Continue to use your skills to try new recipes and stay engaged with your diet. Consider meal prepping for the week to ensure you stick to your healthy eating goals.",
                        "You are doing a great job at including a variety of food groups. Keep it up by adding more nutrient-dense snacks like nuts, seeds, and Greek yogurt, and exploring different cuisines for balanced meals.",
                        "You’re excellent at avoiding fast food and opting for home-cooked meals. To enhance this, try incorporating more international dishes that fit your dietary goals for added variety and enjoyment.",
                        "Your consistent intake of fruits and vegetables is impressive. For added benefits, try including more fermented foods like sauerkraut or kimchi to boost your digestive health.",
                        "Your diet includes a high amount of whole grains and healthy fats. Continue this by adding diverse grains such as farro or barley, and experimenting with different healthy oils like avocado or walnut oil.",
                        "Your approach to healthy eating is exemplary. Maintain your diet by staying informed about nutrition and considering regular consultations with a dietitian to optimize your eating plan."
                    }
                },
                {
                    "23-34 Points (Good)", new string[]
                    {
                        "You have a good foundation with your diet. To improve further, aim to increase your intake of vegetables by adding them to every meal. Experiment with different cooking methods to make veggies more enjoyable.",
                        "Your diet includes a variety of foods, but there's room for improvement. Focus on replacing processed snacks with healthier alternatives like fresh fruit, nuts, or whole-grain crackers.",
                        "You’re doing well with your dietary choices. Increase your consumption of whole grains by substituting refined grains with options like brown rice, oats, or whole-wheat pasta.",
                        "You’re on the right track with meal planning. Enhance your diet by including a wider variety of foods and reducing your intake of sugary or fried items. Try planning your meals for the week to stay organized.",
                        "Your current diet is fairly balanced. To improve, consider adding more plant-based proteins such as beans, lentils, or tofu to your meals. These can be great substitutes for meat.",
                        "You’re making strides with healthy eating. Reduce your fast food consumption by preparing quick and easy meals at home, such as stir-fries or salads.",
                        "You’re doing a good job with your food choices. Incorporate more nutrient-dense snacks into your diet, like Greek yogurt with berries or a handful of almonds.",
                        "Increase the variety in your diet by trying new vegetables or fruits that you haven’t had before. This can help you get a broader range of nutrients.",
                        "Focus on reducing your intake of refined sugars and increasing whole food options. Try using natural sweeteners like honey or maple syrup in place of refined sugars.",
                        "To better support your health goals, consider planning your meals around a variety of food groups and incorporating more fresh and whole ingredients into your diet."
                    }
                },
                {
                    "10-22 Points (Fair)", new string[]
                    {
                        "Your diet needs improvement. Start by increasing your intake of fruits and vegetables. Aim for at least one serving of vegetables in every meal and snack on fruits.",
                        "You might be consuming too many processed foods. Begin incorporating more whole foods into your diet, like fresh vegetables, fruits, and whole grains, and reduce your consumption of sugary snacks.",
                        "To improve your diet, focus on eating more plant-based proteins. Try adding beans or lentils to your meals and exploring simple, healthy recipes.",
                        "Consider improving your meal planning. Plan your meals for the week and prepare healthy options in advance to make it easier to stick to a balanced diet.",
                        "You could benefit from reducing fast food intake. Start by cooking simple meals at home, like grilled chicken with vegetables or a hearty vegetable soup.",
                        "Increase your intake of whole grains and reduce refined grains. Try switching to brown rice, quinoa, or whole-wheat pasta to enhance your diet.",
                        "To better balance your diet, include more diverse foods and reduce reliance on processed or convenience foods. Meal prepping can be a helpful tool.",
                        "Your diet could use more nutrient-dense options. Consider adding nuts, seeds, or legumes to your meals and snacks to improve your overall nutrition.",
                        "Focus on making gradual changes to your eating habits. Start with small adjustments, like adding a salad to your meals or swapping out refined sugars for healthier alternatives.",
                        "Consulting a nutritionist could help you better understand how to improve your dietary habits. They can provide personalized recommendations based on your needs."
                    }
                },
                {
                    "0-9 Points (Poor)", new string[]
                    {
                        "Your diet is in need of significant changes. Start by increasing your intake of vegetables and fruits. Aim to replace processed snacks with fresh options like apples or carrots.",
                        "To improve your diet, focus on reducing processed foods and sugary items. Incorporate whole foods into your meals, such as brown rice, vegetables, and lean proteins.",
                        "Your current eating habits may not be meeting your nutritional needs. Begin by planning and preparing balanced meals that include a variety of food groups, and reduce fast food consumption.",
                        "Consider making major changes to your diet. Start by incorporating more whole grains, fruits, and vegetables into your meals and cutting back on sugary and fried foods.",
                        "To improve your nutrition, aim for a more balanced diet by including a variety of foods. Incorporate lean proteins, whole grains, and a wide range of fruits and vegetables into your meals.",
                        "Your diet could benefit from more structure. Try meal planning to ensure you’re getting a balanced intake of nutrients, and avoid relying on fast food or convenience foods.",
                        "Focus on making healthier food choices. Increase your intake of whole foods, reduce processed food consumption, and consider consulting a nutritionist for guidance.",
                        "To start improving your diet, try making small changes like adding a serving of vegetables to each meal and choosing healthier snacks like nuts or fruits.",
                        "Gradually improve your eating habits by focusing on one change at a time. Begin with increasing your intake of vegetables and fruits, and reduce the consumption of sugary foods.",
                        "Seek advice from a healthcare professional or nutritionist to help you create a more balanced and nutritious eating plan. They can offer personalized guidance based on your dietary needs."
                    }
                }
            }
        }
    };


    public Dictionary<string, Dictionary<string, string[]>> categoryTwoFeedbacks = new Dictionary<string, Dictionary<string, string[]>>
{
    {
        "Health Goals", new Dictionary<string, string[]>
        {
            {
                "25-30 Points (Excellent)", new string[]
                {
                    "You’re excelling in meeting your health goals. Maintain your routine and consider setting new, challenging goals to keep progressing. Regular exercise and a balanced diet are key to continued success.",
                    "Your commitment to health is evident. Keep up with your current practices and consider exploring new health and wellness activities, like advanced fitness classes or nutrition workshops.",
                    "Your dedication to health is impressive. Continue tracking your progress and stay motivated by setting new milestones and celebrating your achievements along the way.",
                    "You’re doing an excellent job with your health goals. Maintain this by regularly reviewing and adjusting your goals to ensure they remain challenging and relevant.",
                    "Your health routines are effective. Continue to optimize your plan by incorporating varied physical activities and regularly reviewing your dietary choices to ensure they align with your goals.",
                    "Your adherence to health goals is strong. To enhance your progress, consider adding more variety to your exercise routine and exploring new ways to stay motivated.",
                    "You’re on the right track with your health goals. Keep up your dedication and consider adding supplementary activities like mindfulness or stress management techniques to further support your well-being.",
                    "Your health management is top-notch. Continue to track your progress and make adjustments as needed to stay on track with your health and fitness goals.",
                    "You’ve shown great commitment to your health goals. Enhance this by incorporating new health practices, such as advanced workouts or dietary adjustments, to continue improving your overall well-being.",
                    "Your dedication to health is commendable. Keep up with your effective routines and consider seeking advice from health professionals to optimize your approach and achieve even greater results."
                }
            },
            {
                "19-24 Points (Good)", new string[]
                {
                    "You’re doing well with your health goals. To improve further, increase the consistency of your exercise routine and consider adding new activities to keep things interesting.",
                    "Your progress is good, but there’s room for improvement. Focus on optimizing your diet and exercise plan by incorporating a wider variety of foods and activities.",
                    "You’re making solid progress. Enhance your health goals by setting specific, achievable milestones and regularly reviewing your progress to stay motivated.",
                    "You have a good foundation. Improve by exploring new fitness routines and adjusting your diet to better align with your health objectives.",
                    "Your health goals are on track. Consider increasing your physical activity levels and incorporating more diverse exercises to further enhance your fitness.",
                    "You’re managing your health well. Try to make your meal planning more varied and reduce reliance on convenience foods by preparing more homemade meals.",
                    "Your health routines are effective. Increase your water intake and make a habit of regularly tracking your nutritional and fitness progress for better results.",
                    "You’re doing well with your goals. Focus on improving your consistency by sticking to a regular exercise schedule and making more balanced food choices.",
                    "Your dedication is good. To further support your health goals, consider setting aside specific times for physical activity and meal planning each week.",
                    "You’re on track with your health goals. Enhance your routine by incorporating more health-focused activities and monitoring your progress closely for continuous improvement."
                }
            },
            {
                "10-18 Points (Fair)", new string[]
                {
                    "To improve your health goals, focus on increasing your physical activity. Start with manageable activities and gradually increase intensity.",
                    "Your current health routine could use some adjustments. Aim to improve your diet by incorporating more whole foods and reducing processed foods.",
                    "Consider enhancing your health goals by setting specific targets for exercise and nutrition. Regularly review your progress and make necessary changes.",
                    "Your health management needs improvement. Increase your water intake, incorporate more vegetables into your meals, and set realistic exercise goals.",
                    "Start by making small changes to your health routine. Focus on increasing your physical activity and gradually improving your diet with more balanced meals.",
                    "Improve your consistency by planning and preparing meals ahead of time. Aim to include a variety of foods and set specific times for exercise each week.",
                    "To better support your health goals, consider incorporating more regular physical activity and tracking your progress with a fitness or nutrition app.",
                    "Focus on gradual improvements to your health routine. Incorporate simple changes like daily walks and healthier meal options to enhance your overall well-being.",
                    "Your current approach to health needs more structure. Start by setting clear, achievable goals for both exercise and diet, and track your progress regularly.",
                    "Seek advice from a health professional to help you better align your routine with your health goals. They can provide personalized recommendations and support."
                }
            },
            {
                "0-9 Points (Poor)", new string[]
                {
                    "Your health goals need significant improvement. Begin by increasing physical activity and making dietary changes, such as incorporating more fruits and vegetables into your meals.",
                    "Focus on making major changes to your routine. Start by planning and preparing balanced meals, and gradually increase your exercise levels for better results.",
                    "Consider making immediate adjustments to your health goals. Increase your physical activity, improve your diet by reducing processed foods, and stay consistent with your efforts.",
                    "Your current approach to health is lacking. Begin by setting up a structured plan that includes regular exercise and balanced meals to improve your overall well-being.",
                    "To start improving your health, focus on making basic changes. Incorporate more nutritious foods into your diet, and establish a regular exercise routine.",
                    "Significant changes are needed. Start by making small, manageable adjustments to your diet and exercise routine, and gradually build up your health practices.",
                    "Your health management could be greatly improved. Begin by setting specific, achievable goals for exercise and nutrition, and seek guidance from health professionals if needed.",
                    "Focus on immediate improvements to your health routine. Incorporate daily physical activity and work on reducing processed and sugary foods in your diet.",
                    "To improve your health, start with small, consistent changes. Incorporate more fruits, vegetables, and whole grains into your meals, and increase your physical activity gradually.",
                    "Seek professional advice to help create a more effective health plan. A healthcare provider can offer personalized recommendations and support to improve your overall health."
                }
            }
        }
    }
};


    public Dictionary<string, Dictionary<string, string[]>> categoryFourFeedbacks = new Dictionary<string, Dictionary<string, string[]>>
{
    {
        "Physical Activity", new Dictionary<string, string[]>
        {
            {
                "25-30 Points (Excellent)", new string[]
                {
                    "Your physical activity routine is outstanding. Regular cardiovascular and strength training, along with daily flexibility exercises and an active lifestyle, reflect a strong commitment to fitness. Keep up the excellent work and continue setting and tracking fitness goals.",
                    "You excel in maintaining a well-rounded fitness routine. Your frequent engagement in cardiovascular and strength training, combined with daily flexibility exercises and high activity levels, showcases your dedication to physical health. Continue your impressive routine.",
                    "Your physical activity habits are exceptional. Regular cardiovascular and strength exercises, flexibility work, and daily active habits indicate a high level of commitment. Keep setting and achieving fitness goals to maintain this excellent routine.",
                    "You have an excellent approach to physical activity. Regular engagement in various types of exercise, high daily activity, and effective goal-setting highlight your commitment to fitness. Continue to explore new activities and stay satisfied with your progress.",
                    "Your physical activity is exemplary. Regular cardiovascular and strength training, combined with flexibility exercises and high daily activity, reflects a strong fitness routine. Maintain your current practices and continue setting and tracking your fitness goals.",
                    "You are doing an excellent job with your physical activity routine. Regular exercise across various types, high daily activity levels, and effective goal-setting are commendable. Continue your impressive routine and explore new activities.",
                    "Your commitment to physical activity is outstanding. Regular exercise, a variety of activities, and high daily activity levels demonstrate a strong fitness routine. Keep up the good work by setting and tracking fitness goals.",
                    "Your approach to physical activity is top-notch. Consistent cardiovascular and strength training, daily flexibility exercises, and a high level of daily activity reflect a strong commitment to fitness. Continue exploring new activities and setting fitness goals.",
                    "You have an exceptional physical activity routine. Regular engagement in cardiovascular and strength exercises, combined with daily flexibility work and high activity levels, showcases your dedication. Maintain this routine and continue tracking your fitness goals.",
                    "Your physical activity habits are exemplary. Consistent exercise across various types, high daily activity levels, and effective goal-setting demonstrate a strong commitment to fitness. Keep up the excellent work and stay motivated to explore new activities."
                }
            },
            {
                "19-24 Points (Good)", new string[]
                {
                    "Your physical activity routine is solid but could be improved. Increase the frequency of cardiovascular and strength training, incorporate more flexibility exercises, and aim to be more active throughout the day. Continue setting and tracking your fitness goals for better results.",
                    "You have a good physical activity routine. To enhance it, focus on increasing the frequency of cardiovascular and strength training, adding more flexibility exercises, and boosting daily activity levels. Setting and tracking fitness goals will help maintain your progress.",
                    "Your physical activity habits are commendable. To improve, increase the regularity of your cardiovascular and strength exercises, add more flexibility work, and ensure higher daily activity. Continue setting and tracking your fitness goals for better outcomes.",
                    "You’re doing well with your physical activity but can benefit from some improvements. Increase the frequency of your cardiovascular and strength training, incorporate more flexibility exercises, and aim for higher daily activity. Keep setting and tracking your fitness goals.",
                    "Your physical activity is good, but there’s room for improvement. Focus on increasing the regularity of cardiovascular and strength exercises, add flexibility work, and be more active throughout the day. Continue to set and track your fitness goals.",
                    "You manage your physical activity reasonably well. To enhance your routine, increase the frequency of cardiovascular and strength training, add more flexibility exercises, and boost your daily activity levels. Set and track fitness goals to stay on track.",
                    "Your approach to physical activity is decent. To improve, incorporate more cardiovascular and strength training, include flexibility exercises, and aim for higher daily activity. Setting and tracking fitness goals will support your progress.",
                    "Your physical activity routine is good but could use some adjustments. Increase the frequency of your cardiovascular and strength exercises, add flexibility work, and aim for higher daily activity. Continue to set and track your fitness goals.",
                    "You’re on the right track with your physical activity but can enhance it by incorporating more cardiovascular and strength training, increasing flexibility exercises, and being more active throughout the day. Setting and tracking fitness goals will be beneficial.",
                    "Your physical activity habits are satisfactory. Focus on improving by increasing the frequency of cardiovascular and strength training, adding more flexibility exercises, and aiming for higher daily activity. Continue setting and tracking your fitness goals."
                }
            },
            {
                "10-18 Points (Fair)", new string[]
                {
                    "Your physical activity routine needs improvement. Start by increasing the frequency of cardiovascular and strength training, adding more flexibility exercises, and aiming to be more active throughout the day. Setting and tracking fitness goals will help.",
                    "To enhance your physical activity routine, focus on increasing cardiovascular and strength training, incorporating more flexibility exercises, and boosting daily activity levels. Regular goal-setting and tracking will support your progress.",
                    "Your current physical activity habits require adjustments. Begin by incorporating more cardiovascular and strength exercises, adding flexibility work, and being more active daily. Setting and tracking fitness goals will be beneficial.",
                    "You need to make significant changes to your physical activity routine. Start by increasing cardiovascular and strength training, including more flexibility exercises, and improving daily activity levels. Continue setting and tracking your fitness goals.",
                    "To improve your physical activity, focus on increasing the frequency of your workouts, incorporating a mix of cardiovascular, strength, and flexibility exercises, and being more active throughout the day. Setting and tracking goals will aid in your progress.",
                    "Your physical activity needs significant improvement. Begin by adding more cardiovascular and strength exercises, increasing flexibility work, and aiming for higher daily activity. Regular goal-setting and tracking will help maintain progress.",
                    "Your approach to physical activity requires changes. Start by increasing the frequency of your cardiovascular and strength training, adding flexibility exercises, and boosting your daily activity. Setting and tracking fitness goals will support your routine.",
                    "You need to make improvements in your physical activity habits. Focus on increasing cardiovascular and strength training, incorporating more flexibility exercises, and enhancing daily activity levels. Regularly set and track your fitness goals.",
                    "Your physical activity routine needs work. Begin by incorporating more cardiovascular and strength exercises, adding flexibility work, and aiming for higher daily activity. Continue to set and track your fitness goals for better outcomes.",
                    "Significant changes are needed in your physical activity routine. Increase cardiovascular and strength training, include flexibility exercises, and aim for more daily activity. Setting and tracking fitness goals will support your improvement."
                }
            },
            {
                "0-9 Points (Poor)", new string[]
                {
                    "Your physical activity routine needs major improvements. Start by incorporating basic cardiovascular and strength exercises, adding flexibility work, and increasing your daily activity levels. Set and track fitness goals to guide your progress.",
                    "To improve your physical activity, begin with basic exercises like cardiovascular, strength, and flexibility workouts. Increase your daily activity and set fitness goals to support your progress.",
                    "Significant changes are needed in your physical activity habits. Start by including basic cardiovascular and strength exercises, adding flexibility work, and improving daily activity levels. Regularly set and track fitness goals.",
                    "Your physical activity needs a complete overhaul. Incorporate basic cardiovascular and strength exercises, add flexibility work, and aim to be more active throughout the day. Setting and tracking fitness goals will help guide your improvement.",
                    "To enhance your physical activity, start by incorporating essential cardiovascular and strength exercises, increasing flexibility work, and improving your daily activity levels. Set and track fitness goals for better results.",
                    "Immediate improvements are needed in your physical activity routine. Begin with basic cardiovascular and strength exercises, add flexibility work, and increase daily activity. Regularly set and track your fitness goals.",
                    "Your physical activity routine requires significant changes. Start by incorporating more basic exercises, including cardiovascular, strength, and flexibility workouts, and increase daily activity. Set and track fitness goals for better progress.",
                    "To start improving your physical activity, focus on basic exercises like cardiovascular and strength training, add flexibility work, and boost your daily activity levels. Setting and tracking fitness goals will support your improvement.",
                    "Your physical activity habits need major adjustments. Begin with incorporating basic exercises, increasing flexibility work, and improving daily activity levels. Set and track fitness goals to guide your progress.",
                    "Your physical activity routine requires major changes. Start by adding essential cardiovascular and strength exercises, incorporating flexibility work, and aiming for more daily activity. Regular goal-setting and tracking will help improve your routine."
                }
            }
        }
    }
};

    

    public string GetFeedback(int categoryIndex, int score)
    {
        // Select the feedback dictionary based on the category index
        Dictionary<string, Dictionary<string, string[]>> feedbackDictionary = GetFeedbackDictionary(categoryIndex);

        if (feedbackDictionary == null)
        {
            return "Invalid category.";
        }

        // Convert categoryIndex to the category name (assumes same category names for each dictionary)
        string categoryName = GetCategoryName(categoryIndex);
        string scoreRange;
        if(categoryIndex==0){
            // Determine the score range
            scoreRange = GetScoreRange(score);
        }
        else{
            scoreRange = GetScoreRange1(score);
        }

        

        // Retrieve feedback based on category and score range
        if (feedbackDictionary.TryGetValue(categoryName, out var feedbacks) &&
            feedbacks.TryGetValue(scoreRange, out var feedbackArray))
        {
            // Select random feedback
            int randomIndex = Random.Range(0, feedbackArray.Length);
            return feedbackArray[randomIndex];
        }
        else
        {
            return "No feedback available for this score.";
        }
    }

    private Dictionary<string, Dictionary<string, string[]>> GetFeedbackDictionary(int categoryIndex)
    {
        // Return the appropriate feedback dictionary based on the category index
        switch (categoryIndex)
        {
            case 0:
                return categoryOneFeedbacks;
            case 1:
                return categoryTwoFeedbacks;
            case 2:
                return categoryFourFeedbacks;
            default:
                return null;
        }
    }

    private string GetCategoryName(int categoryIndex)
    {
        // Return category name based on index
        switch (categoryIndex)
        {
            case 0:
                return "Dietary Preferences";
            case 1:
                return "Health Goals"; // Update with actual names
            case 2:
                return "Physical Activity"; // Update with actual names
            default:
                return "Unknown Category";
        }
    }

    private string GetScoreRange(int score)
    {
        // Return score range based on score
        if (score >= 35) return "35-40 Points (Excellent)";
        else if (score >= 23) return "23-34 Points (Good)";
        else if (score >= 10) return "10-22 Points (Fair)";
        else if (score >= 0) return "0-9 Points (Poor)";
        else return "Invalid Score";
    }
    private string GetScoreRange1(int score)
    {
        // Return score range based on score
        if (score >= 25) return "25-30 Points (Excellent)";
        else if (score >= 19) return "19-24 Points (Good)";
        else if (score >= 10) return "10-18 Points (Fair)";
        else if (score >= 0) return "0-9 Points (Poor)";
        else return "Invalid Score";
    }
}