**Q1: How long did you spend on the coding assignment? What would you add to your solution if you had
more time? If you didn't spend much time on the coding assignment then use this as an opportunity to explain what you would add.**

> **Tasks Breakdown**
>  * Deciding on the used technologies / language (1 Hour)
>  * Identifying Challenges (1 Hour)
>  * Acquiring API Keys from Provided APIs and Understanding Pricing Tiers and Documentation (2 Hours)
>  * High Level Design of the solution (1 Hour)
>  * Low Level Design and Implementation (8 Hours)
>  * Writing Unit Tests (2 Hours)
>   * *Total (15 Hours)* 
> 
> **If I had more time I would add**
>  * Support for Docker
>  * Support for Redis Cache
>  * Performance Monitoring Tools (APM)
>  * Better Exception Handling and Support for Logging tools (ELK)  
>  * User Authentication / Authorization
>  * Add Integration Tests
>  * Add Failover and Retry Mechanism
>  * CI/CD Pipeline

---
   **Q2: What was the most useful feature that was added to the latest version of your language of choice?
Please include a snippet of code that shows how you've used it.**

> * .NET 6 Minimal API
> * ![image](https://user-images.githubusercontent.com/4711491/177818547-78b9a3f8-c15f-4fd0-9bd8-9868bd4c86b0.png)

> * Global Using Statements
> * ![image](https://user-images.githubusercontent.com/4711491/177818881-ce71559d-aa66-450f-8863-7458a7385f1c.png)

---

**Q3: How would you track down a performance issue in production? Have you ever had to do this?**

> There are some steps we can follow to narrow down our search and easily identify the root cause of the issue
> * If the application is behind a load balancer: we need to check if all instances are up and running, if any instance are down we need bring it up, we can make also use of Health Check endpoints to keep track of the instances health status and setup alerts for unhealthy instances as well.
> * Check if any recent deployment happened that might caused this issue
> * Check CPU and Memory Utilization, if we have any spike. we need to check if any pattern related to some functionalities and start troubleshooting this functionality.
> * Check if there is any Memory Leaks, using tools like (dotnet-counters, dotMemory, etc) 
> * Check if there is any Expensive / Slow SQL queries using tools like (Elastic APM, newrelic APM, SQL Server Profiler).
> * Check application / server logging to identify the issue exact location using tools like (Google Stackdriver, AWS CloudWatch, etc.)
> - I experienced some performance issues and usually identified and solved them using one the above mentioned tools. 

---

**Q4: What was the latest technical book you have read or tech conference you have been to? What did you
learn?**

> The last book I read (not recently) was **Designing Distributed Systems: Patterns and Paradigms for Scalable, Reliable Services** , this was a great book that covered many topics and challenges you face while building distributed systems, things like scalability, consistency, availability and resilience.
> Currently, started to read another great book **Designing Data-Intensive Applications**, which deeply covers the same topics .

---

**Q5: What do you think about this technical assessment?**
> In my opinion the task was very interesting as it covered different aspects and it has some tricky parts that will challenge some developers.

---

**Q6: Please, describe yourself using JSON.**
```json
{
    "personal": {
      "details": {
        "first_name": "Peter",
        "last_name": "Habib",
        "email": "prog.peter.habib@gmail.com",
        "age": 33
      },
      "family": {
        "marital_status": "Married",
        "kids": [
          "Tia",
          "Elie"
        ]
      },
      "hobbies": [
        "Reading Books",
        "Computer Hardware"
      ]
    },
    "work": {
      "title": "Lead Software Engineer",
      "company": "Namshi.com",
      "experience": {
        "years": 12,
        "top_skills": [
          "dotNET",
          "AWS",
          "GCP",
          "NodeJS",
          "Docker",
          "MS Dynamics 365",
          "Redis"
        ]
      },
      "certificates": [
        "AWS Certified Solutions Architect",
        "API Designer",
        "Microsoft Certified Professional"
      ]
    }
  }
```
