import java.util.*;

public class Elevator {
    String name;
    int currentFloor;
    String direction; // "up", "down", or "idle"
    List<Person> people;
    Set<Integer> stops;

    final int MAX_PEOPLE = 20;
    final int MAX_WEIGHT = 30000;

    public Elevator(String name) {
        this.name = name;
        this.currentFloor = 0;
        this.direction = "idle";
        this.people = new ArrayList<>();
        this.stops = new TreeSet<>();
    }

    public int currentWeight() {
        return people.stream().mapToInt(p -> p.weight).sum();
    }

    public boolean canAdd(Person person) {
        return people.size() < MAX_PEOPLE && currentWeight() + person.weight <= MAX_WEIGHT;
    }

    public boolean addPerson(Person person) {
        if (canAdd(person)) {
            people.add(person);
            stops.add(person.destinationFloor);
            person.inElevator = true;
            return true;
        }
        return false;
    }

    public void move() {
        if ("up".equals(direction)) {
            currentFloor++;
        } else if ("down".equals(direction)) {
            currentFloor--;
        }
    }

    public List<Person> dropOff() {
        List<Person> exiting = new ArrayList<>();
        Iterator<Person> iterator = people.iterator();

        while (iterator.hasNext()) {
            Person p = iterator.next();
            if (p.destinationFloor == currentFloor) {
                exiting.add(p);
                iterator.remove();
            }
        }

        stops.remove(currentFloor);
        return exiting;
    }

    public void setDirection() {
        if (people.isEmpty()) {
            direction = "idle";
        } else {
            boolean hasUp = people.stream().anyMatch(p -> p.destinationFloor > currentFloor);
            boolean hasDown = people.stream().anyMatch(p -> p.destinationFloor < currentFloor);
            if (hasUp) direction = "up";
            else if (hasDown) direction = "down";
        }
    }
}
