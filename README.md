# NewsAggregator
## Idea
The purpose of the system is to fetch the latest news for a user, using the prefered subjects entered.

Each user will have an account and will be able to change the preferences at any time.

## Design
```mermaid
    graph Design;
    NewsData.io<-->NewsAPI;
    NewsAPI<-->Manager;
    Manager<-->GeminiAPI;
    GeminiAPI<-->Gemini;
    Manager<-->EmailAPI;
    EmailAPI<-->EmailProvider;
    Manager<-->UserAccessor;
    UserAccessor<-->MySQL;
```