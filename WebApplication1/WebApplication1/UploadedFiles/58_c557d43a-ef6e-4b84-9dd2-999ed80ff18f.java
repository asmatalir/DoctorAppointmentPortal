import java.util.*;

public class Main {
    public static void main(String[] args) {
        List<Person> waitingList = new ArrayList<>();

        // Sample People
        waitingList.add(new Person(1, 0, 10, 70));
        waitingList.add(new Person(2, 0, 20, 75));
        waitingList.add(new Person(3, 0, 30, 80));
        waitingList.add(new Person(4, 5, 50, 65));
        waitingList.add(new Person(5, 5, 15, 90));
        waitingList.add(new Person(6, 10, 40, 85));
        waitingList.add(new Person(7, 20, 5, 95));

        List<Elevator> elevators = Arrays.asList(
            new Elevator("A"),
            new Elevator("B"),
            new Elevator("C")
        );

        simulateElevators(waitingList, elevators);
    }

    public static void simulateElevators(List<Person> waitingList, List<Elevator> elevators) {
        int time = 0;
        while (!waitingList.stream().allMatch(p -> p.inElevator && p.destinationFloor == getPersonFloor(p, elevators))) {
            System.out.println("\n--- Time Step: " + time + " ---");

            assignPeople(waitingList, elevators);

            for (Elevator elevator : elevators) {
                elevator.setDirection();
                elevator.move();
                List<Person> dropped = elevator.dropOff();

                if (!dropped.isEmpty()) {
                    System.out.print("Elevator " + elevator.name + " dropped off: ");
                    for (Person p : dropped) {
                        System.out.print("P" + p.id + " ");
                    }
                    System.out.println("at floor " + elevator.currentFloor);
                }
            }

            time++;
        }

        System.out.println("\n✅ All people reached their destination!");
    }

    public static int getPersonFloor(Person p, List<Elevator> elevators) {
        for (Elevator e : elevators) {
            for (Person ep : e.people) {
                if (ep.id == p.id) {
                    return e.currentFloor;
                }
            }
        }
        return p.destinationFloor; // dropped off
    }

    public static void assignPeople(List<Person> waitingList, List<Elevator> elevators) {
        for (Person person : waitingList) {
            if (person.inElevator) continue;

            // Sort elevators by proximity to person’s current floor
            elevators.sort(Comparator.comparingInt(e -> Math.abs(e.currentFloor - person.currentFloor)));

            for (Elevator elevator : elevators) {
                if (elevator.currentFloor == person.currentFloor && elevator.canAdd(person)) {
                    elevator.addPerson(person);
                    System.out.println("Person " + person.id + " entered Elevator " + elevator.name + " at floor " + person.currentFloor);
                    break;
                }
            }
        }
    }
}
