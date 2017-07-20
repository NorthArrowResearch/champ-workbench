---
title: Custom Visits
---

The primary purpose of the CHaMP Workbench is to help users work with the existing, pre-defined visits associated with the CHaMP program. The official set of visits is defined on the central [CHaMP Monitoring](https://www.champmonitoring.org) database and then [synchronized]({{ site.url }}/Technical_Reference/Data_Menu/synchronize_champ_data.html) down the the local Workbench database.

However, it is possible to create visits that represent unofficial field surveys and store these in the local Workbench databaes and use the software's features to manipulate them. These so called *custom visits* are stored in the local workbench database only and never uploaded or synchronized with the central CHaMP systems.

![Custom Visits]({{ site.url }}/assets/images/custom_visits.png)

## Creating Custom Visits

1. Click `Data` on the main menu and choose `Create Custom Visit...`
1. Provide a **Visit ID**. This **must** be unique and distinct from any other visit ID in the local Workbench database. Given that CHaMP periodically issues new visit IDs (they started at 1 and are up to ~ 4,000 for the 2017 field season) it is strongly advised that you pick a visit number that is easily distinguishable from those issued by the CHaMP program. The default is 9000 for this reason, but you could also choose a larger number still.
1. Specify the **field season** (calendar year) in which the survey was collected.
1. Pick one of the existing **watersheds** or enter the name of a custom watershed.
1. Pick one of the existing **sites** or enter the name of a custom site where the visit took place.
1. Pick a **program**. This designation is not critical and so you can choose any existing program as a placeholder.
1. Select a **protocol** (this is optional).
1. Specify the name of the organization that collected the survey.
1. If you intend to calculate topographic metrics for the custom visit then you will need to specify the channel units for the visit. See below.

## Channel Units

## Demonstration


<div class="flex-video">
 <iframe src="https://www.youtube.com/embed/_lN4nVq1OVQ" frameborder="0" allowfullscreen=""></iframe>
</div>