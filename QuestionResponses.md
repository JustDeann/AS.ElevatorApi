# Question Responses
This document is for the written part of the coding assessment. 
## 1. Please describe the differences between IAAS, PAAS and SAAS and give examples of each in a cloud platform of your choosing?
**Most systems use all three or a hybrid combination and is determined by requirement need and cost.** 

### Infrastructure as a Service (IaaS)

This gives you access to virtualized infrastructure resources such as servers, storage, and networking. The provider manages the physical hardware, basic networking, and data center operations, while you install and manage the operating system, runtime, middleware, and the application code. IaaS offers the flexibility to scale resources up or down as requirements change, helping you optimize performance and manage costs. Examples include Azure Virtual Machines, Azure Load Balancer, and storage services like Azure Managed Disks.

### Platform as a Service (PaaS)

This provides a managed platform for developing, running, and deploying applications without needing to manage the underlying infrastructure. The provider handles the operating system, runtime, middleware, and platform updates, while you focus on writing and managing your application code and data. PaaS speeds up development by giving you a ready-to-go environment and is ideal for collaborative, shared development. Examples include Azure App Service, Azure SQL Database, and Azure Front Door.

### Software as a Service (SaaS)

This delivers fully managed application that you access over the internet. The provider manages everything while you simply use the software. SaaS requires no installation or maintenance and allows users to start working immediately. Examples include Microsoft 365, Dynamics 365, Salesforce, and Azure DevOps Services.

---

## 2. What are the considerations of a build or buy decision when planning and choosing software?

When deciding whether to build or buy software, I consider the project scope, budget, and timeline to determine whether an existing solution can deliver faster. I evaluate total cost of ownership, required customization, and how well each option fits the problem. Any purchased software must meet security and compliance requirements, provide regular updates and vendor support, and integrate well with existing systems. I also compare multiple software options and review what other teams in the organization already use to ensure consistency and avoid redundant tools. I had to do this in Alaska Cargo IT, we had to end up getting a .Net library for PDF creation. I went through the process of a Spike then DevSecOps security review.  

---

## 3. What are the foundational elements and considerations when developing a serverless architecture? 
Foundational elements of a serverless architecture include event-driven compute (like functions), managed services for data storage, messaging, and authentication, and an API layer for exposing functionality. Considerations include understanding the event triggers, designing for stateless execution, and choosing services that scale. You must plan for cold starts, concurrency limits, and cost optimization based on usage. Security and reliability are also critical, as well as, central logging with IDs so you can trace the request flow. 

---

## 4. Please describe the concept of composition over inheritance.

Composition over inheritance is a design principle that encourages building complex behavior by combining small, reusable components rather than relying on inheritance hierarchies. Instead of extending a base class to gain features, you assemble objects that each handle a very specific responsibility. This makes code more flexible, easier to test, and less tightly coupled.


---

## 5. Describe a design pattern you've used in production code. What was the pattern? How did you use it? Given the same problem how would you modify your approach based on your experience?

One design pattern I've used in production code is the Repository Pattern, which appears in my BlogListQueryRepository for CG.CargoCMS.KX13 (which is in Cargo IT) blog component implementation. The goal of this pattern is to separate data-access logic from business logic by providing a clean abstraction for querying and transforming data.

The Repository Pattern abstracts the data source and provides a consistent interface for retrieving domain models. In this case, IBlogListQueryRepository defines the contract, and BlogListQueryRepository implements the logic for retrieving content, handling caching, logging failures, and converting CMS page types into strongly typed response models.

I used the repository to encapsulate all Kentico blog page type retrieval operations. The consumer of the repository doesn't need to know anything about Kentico APIs, query syntax, caching rules, or how to construct BlogPost or BlogListQueryResponse objects. The GetBlogList method centrally manages caching via CacheHelper.CacheAsync, and GetBlogListUncached handles the raw data retrieval and cache dependencies. The ConvertPagesToResponse method transforms CMS page types into domain models, keeping the mapping logic contained and testable. This kept the controller and business layers clean and allowed the rest of the system to depend on the interface rather than the CMS. We did this in order to create custom page types, widgets and sections that the administrator would use to customize the page. 

Overall, the Repository Pattern worked well for abstracting the CMS implementation details, but with hindsight, I'd refactor the mapping using AutoMapper and take it out of the repository to make it cleaner and easier to test.