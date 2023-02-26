using UnityEngine;
using UnityEngine.AI;

public class MouseMoveTest : MonoBehaviour
{

    public Transform goal;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = goal.position;

        if (agent.isOnOffMeshLink)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;

            //calculate the final point of the link
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

            //Move the agent to the end point
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);

            //when the agent reach the end point you should tell it, and the agent will "exit" the link and work normally after that
            if (agent.transform.position == endPos)
            {
                agent.CompleteOffMeshLink();
            }
        }

    }
}